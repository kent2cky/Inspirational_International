using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Inspiration_International.Identity;

namespace Inspiration_International.Identity
{
    public class MyApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public MyApplicationDbContext(DbContextOptions<MyApplicationDbContext> options)
        : base(options)
        { }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            // For Guid Primary Key
            builder.Entity<ApplicationUser>().Property(p => p.Id).ValueGeneratedOnAdd();
        }


        public DbSet<Inspiration_International.Identity.ApplicationUser> ApplicationUser { get; set; }

    }
}