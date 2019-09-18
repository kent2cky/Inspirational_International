using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Inspiration_International.Repositories;
using Inspiration_International.Models;
using Microsoft.AspNetCore.Identity;
using Inspiration_International.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Inspiration_International.Controllers
{
    public class HomeController : Controller
    {
        public IConfiguration _configuration { get; set; }
        public UserManager<ApplicationUser> _userManager { get; set; }
        public SignInManager<ApplicationUser> _signInManager { get; set; }
        public IArticlesRepo _articlesRepo { get; set; }
        public IRSVPRepo _rsvpRepo { get; set; }
        public ICommentsRepo _commentsRepo { get; set; }
        public HomeController(IArticlesRepo articlesRepo, IRSVPRepo rSVPRepo, UserManager<ApplicationUser> userManager,
                SignInManager<ApplicationUser> signInManager, ICommentsRepo commentsRepo)
        {
            _articlesRepo = articlesRepo;
            _rsvpRepo = rSVPRepo;
            _commentsRepo = commentsRepo;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public class Param
        {
            public string RSVP { get; set; }
        }

        // [Authorize]
        public async Task<IActionResult> Index(Param RSVP = null)
        {
            try
            {
                // var v = await _commentsRepo
                // // .UpdateCommentAsync(3, Convert.ToDateTime("2019-08-16T18:51:26.000"),
                // // "A great article Sir. I need more.", "Coach Victor", 199);
                // //.DeleteRSVPAsync(-1);
                // .SubmitCommentAsync(
                //     DateTime.Now,
                //     "08035401681",
                //     "Coach Oris",
                //     7
                // );
                // Console.WriteLine($"{v}"); //.CommentBody}\n");
                // .GetSingleRSVPByIDAsync(1);
                // Console.WriteLine($"{v.DidAttend}, {v.Name}, {v.DateFor}");
                // .GetAllRSVPsAsync();
                //.GetArticlesByDatePostedAsync(Convert.ToDateTime("2019-02-10"));
                // foreach (var comment in v)
                // {
                //     Console.WriteLine($"{comment.CommentID} {comment.Name} said {comment.CommentBody} on {comment.DateTimePosted}\n");
                // }


                var model = new RSVPViewModel();
                if (User.Identity.IsAuthenticated)
                {
                    ApplicationUser user = null;

                    user = await _userManager.FindByNameAsync(User.Identity.Name);
                    model.FirstName = user.FullName.Split(" ")[0];
                    model.PhoneNumber = user.PhoneNumber;

                }
                var identity = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                return View(model);
            }
            catch
            {
                return View();
            }

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}
