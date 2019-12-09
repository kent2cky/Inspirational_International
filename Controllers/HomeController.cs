// using System;
// using Microsoft.AspNetCore.Identity;

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
using Microsoft.Extensions.Logging;
using System.Text;
using System.Runtime.InteropServices;
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
        public ILogger<HomeController> _logger { get; set; }
        public HomeController(IArticlesRepo articlesRepo, IRSVPRepo rSVPRepo, UserManager<ApplicationUser> userManager,
                SignInManager<ApplicationUser> signInManager, ILogger<HomeController> logger, ICommentsRepo commentsRepo)
        {
            _articlesRepo = articlesRepo;
            _rsvpRepo = rSVPRepo;
            _commentsRepo = commentsRepo;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
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

                var v = HttpContext.Session.GetString("_dateOfNextClass");
                Console.WriteLine($"\n\n\n\n{v}\n\n\n\n");
                if (v == null)
                {
                    var nextClass = JsonConvert.SerializeObject(DateTime.UtcNow).ToString();
                    Console.WriteLine($"\n\n\n\n{nextClass.ToString()} dfjasl;dfjs \n\n\n\n");
                    HttpContext.Session.SetString("_dateOfNextClass", nextClass);
                }

                v = HttpContext.Session.GetString("_dateOfNextClass");
                Console.WriteLine($"\n\n\n\n{v}\n\n\n\n");


                bool isWindows = System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
                bool isLinux = System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

                // foreach (var TimeZone in TimeZoneInfo.GetSystemTimeZones())
                // {
                //     Console.Error.WriteLine(TimeZone.Id + "\t\t " + TimeZone + "\n");
                //     Console.WriteLine(System.Runtime.InteropServices.RuntimeInformation.OSArchitecture.ToString());
                //     Console.WriteLine(System.Runtime.InteropServices.RuntimeInformation.OSDescription.ToString());
                //     Console.WriteLine("Is window: " + isWindows.ToString());
                //     Console.WriteLine("Is Linux: " + isLinux.ToString());
                //     Console.WriteLine(HttpContext.Session.GetString("_dateOfNextClass").ToString());
                // }

                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return View();
            }

        }

        [HttpGet]
        public async Task<JsonResult> GetViewModel([FromQuery] string ls)
        {
            var viewModel = new RSVPViewModel();
            if (ModelState.IsValid)
            {
                if (User.Identity.IsAuthenticated)
                {
                    var user = await _userManager.FindByNameAsync(User.Identity.Name);

                    // Check if the user has RSVPd for the next class.
                    // and if so get the user's record.
                    var rsvp = await _rsvpRepo.GetSingleRSVPByUserIDAndDateForAsync(
                                user.Id.ToString(),
                                DateTime.Now.Next(DayOfWeek.Sunday)
                            );

                    var nextClass = DateTime.Now.Next(DayOfWeek.Sunday);
                    // If user is in list and the date is date of next class
                    // set RSVP to true else set it to false
                    if (rsvp != null && (rsvp.DateFor.Date == nextClass.Date))
                    {
                        viewModel.RSVP = true;
                    }
                    else
                    {
                        viewModel.RSVP = false;
                    }

                    viewModel.PhoneNumber = user.PhoneNumber != null ? "true" : "false";
                    viewModel.FirstName = user.FullName.Split(" ")[0];
                }
                Console.WriteLine("\n\n\n\n\n\n\n\n" + viewModel + "\n\n\n\n\n\n\n\n\n");
                return Json(viewModel);
            }
            _logger.LogError("Error sending viewModel");
            return Json(viewModel); // return empty viewModel.
        }

        [HttpPost]
        // recieve error reports from frontend and log them. 
        public async Task<IActionResult> sendErrorReports([FromBody] FrontEndErrorReportModel error)
        {
            if (ModelState.IsValid)
            {
                _logger.LogError($"Error recieved from {error.Sender}: {error.ErrorMessage}");
                return Ok("Error recieved and logged.");
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


        public async Task<IActionResult> About()
        {
            return View();
        }

        public async Task<IActionResult> GetInspired()
        {
            return View();
        }

        public async Task<IActionResult> ProfessionalCourses()
        {
            return View();
        }

        public async Task<IActionResult> AcquireSkill()
        {
            return View();
        }

        public async Task<IActionResult> GetProfessionalCV()
        {
            return View();
        }

    }
}
