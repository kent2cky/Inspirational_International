using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Inspiration_International.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.CookiePolicy;
using System.IO;
using NLog.Web;
using Inspiration_International.Repositories;
using Microsoft.AspNetCore.Authentication;
using Inspiration_International.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Inspiration_International.Services;
using Microsoft.AspNetCore.Authentication.OAuth;
using System.Security.Claims;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Inspiration_International
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment enviroment)
        {
            _configuration = configuration;
            _enviroment = enviroment;
        }

        public IHostingEnvironment _enviroment { get; }

        public IConfiguration _configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the containers.
        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureQuartzJobsIoc(services);

            services.AddDbContext<MyApplicationDbContext>(options =>
                options.UseSqlServer(
                    _configuration["Secrets:ConnectionString"]));
            services.AddIdentity<ApplicationUser, ApplicationRole>(config =>
            {
                config.SignIn.RequireConfirmedEmail = true;
            })
                .AddEntityFrameworkStores<MyApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddTransient<IEmailSender, EmailSender>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.Configure<CookiePolicyOptions>(options =>
                {
                    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                    options.CheckConsentNeeded = context => false;
                    options.MinimumSameSitePolicy = SameSiteMode.None;
                    options.HttpOnly = HttpOnlyPolicy.None;
                    options.Secure = _enviroment.IsDevelopment() ?
                    CookieSecurePolicy.None : CookieSecurePolicy.Always;
                });

            services.AddAuthentication()
            .AddFacebook("Facebook",
                Facebookoptions =>
                {
                    Facebookoptions.AppId = _configuration["Secrets:Facebook:AppId"];
                    Facebookoptions.AppSecret = _configuration["Secrets:Facebook:AppSecret"];
                }
            )
            .AddGoogle(options =>
            {
                options.ClientId = _configuration["Secrets:Google:ClientID"];
                options.ClientSecret = _configuration["Secrets:Google:ClientSecret"];
                options.ClaimActions.MapJsonKey("urn:google:picture", "picture", "url");
                options.ClaimActions.MapJsonKey("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", "Fullname", "string");
                options.SaveTokens = true;

                options.Events.OnCreatingTicket = ctx =>
                {
                    List<AuthenticationToken> tokens = ctx.Properties.GetTokens().ToList();

                    tokens.Add(new AuthenticationToken()
                    {
                        Name = "TicketCreated",
                        Value = DateTime.UtcNow.ToString()
                    });

                    ctx.Properties.StoreTokens(tokens);
                    return Task.CompletedTask;
                };

            });
            // .AddTwitter();

            services.Configure<IdentityOptions>(options =>
                {
                    // Password settings.
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequiredLength = 4;
                    options.Password.RequiredUniqueChars = 0;

                    // Lockout settings.
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                    options.Lockout.MaxFailedAccessAttempts = 5;
                    options.Lockout.AllowedForNewUsers = true;

                    // User settings.
                    options.User.AllowedUserNameCharacters =
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                    options.User.RequireUniqueEmail = true;
                });

            services.ConfigureApplicationCookie(options =>
                {
                    // Cookie settings
                    options.Cookie.HttpOnly = true;
                    options.ExpireTimeSpan = TimeSpan.FromDays(40);

                    options.LoginPath = "/Identity/Account/Login";
                    options.AccessDeniedPath = "/Identity/AccessDenied";
                    options.SlidingExpiration = true;
                });

            services.AddTransient<IArticlesRepo, ArticlesRepo>();
            services.AddTransient<IRSVPRepo, RSVPRepo>();
            services.AddTransient<ICommentsRepo, CommentsRepo>();


            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                // Set a short timeout for easy testing.
                options.IdleTimeout = TimeSpan.FromMinutes(1);
                options.Cookie.HttpOnly = true;
                // Make the session cookie essential
                options.Cookie.IsEssential = true;
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime lifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // add nlog.config file
            env.ConfigureNLog("nlog.config");

            StartQuartzJobs(app, lifetime);

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseSession();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });


#if DEBUG
            try
            {
                File.WriteAllText("browsersync-update.txt", DateTime.Now.ToString());
            }
            catch
            {
                // ignore
            }
#endif

        }

        private void ConfigureQuartz(IServiceCollection services, params Type[] jobs)
        {
            services.AddSingleton<IJobFactory, IntegrationJobFactory>();
            services.Add(jobs.Select(jobType => new ServiceDescriptor(jobType, jobType, ServiceLifetime.Singleton)));

            services.AddSingleton(provider =>
            {
                var schedulerFactory = new StdSchedulerFactory();
                var scheduler = schedulerFactory.GetScheduler().Result;
                scheduler.JobFactory = provider.GetService<IJobFactory>();
                scheduler.Start();
                return scheduler;
            });
        }

        protected void ConfigureQuartzJobsIoc(IServiceCollection services)
        {
            ConfigureQuartz(services, typeof(MyEmailDispatcher)); //* other jobs come here */
        }

        protected void StartQuartzJobs(IApplicationBuilder app, IApplicationLifetime lifetime)
        {
            var scheduler = app.ApplicationServices.GetService<IScheduler>();
            //TODO: use some config
            QuartzServicesUtilities.StartJob<MyEmailDispatcher>(scheduler, "TimeSpan.FromSeconds(60)");

            lifetime.ApplicationStarted.Register(() => scheduler.Start());
            lifetime.ApplicationStopping.Register(() => scheduler.Shutdown());
        }


    }
}
