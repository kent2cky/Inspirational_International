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
using Microsoft.AspNetCore.Http;
using Inspiration_International.Helpers;
using Newtonsoft.Json;

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
        public async Task<IActionResult> Index()
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
                var rsvpViewModel = new RSVPViewModel();


                var dateOfNextClass = DateTime.Now.Next(DayOfWeek.Sunday);


                //var phoneNumber = TempData["_PN"].ToString();
                rsvpViewModel.FirstName = "Kennis";//TempData["_FN"].ToString() ?? "";
                rsvpViewModel.RSVP = false;//Request.Cookies["_rsvp"] == "true" ? true : false;
                rsvpViewModel.PhoneNumber = "true";//phoneNumber == "true" ? phoneNumber : "false";
                rsvpViewModel.DateOfNextClass = dateOfNextClass;

                return View(rsvpViewModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return View();
            }

        }

        public async Task<IActionResult> SetSession([FromQuery] string LS)
        {
            if (ModelState.IsValid)
            {
                HttpContext.Session.SetString("hasPhoneNumber", LS);
                return Ok();
            }
            return BadRequest();
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
