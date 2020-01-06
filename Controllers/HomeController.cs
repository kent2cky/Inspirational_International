// using System;
// using Microsoft.AspNetCore.Identity;

using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Inspiration_International.Repositories;
using Inspiration_International.Models;
using Microsoft.AspNetCore.Identity;
using Inspiration_International.Identity;
using Microsoft.AspNetCore.Http;
using Inspiration_International.Helpers;
using Microsoft.Extensions.Logging;
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

        public IActionResult Index()
        {
            try
            {
                // if (User.IsInRole("Admin"))
                // {
                //     Console.WriteLine("User is an admin!");
                // }

                SetDateOfNextClassToSession();

                // bool isWindows = System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
                // bool isLinux = System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

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
                _logger.LogError(ex.ToString());
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
                return Json(viewModel);
            }
            _logger.LogError("Error sending viewModel");
            return Json(viewModel); // return empty viewModel.
        }

        [HttpPost]
        // recieve error reports from frontend and log them. 
        public IActionResult sendErrorReports([FromBody] FrontEndErrorReportModel error)
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
            SetDateOfNextClassToSession();
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        public IActionResult About()
        {
            SetDateOfNextClassToSession();
            return View();
        }

        public IActionResult GetInspired()
        {
            SetDateOfNextClassToSession();
            return View();
        }

        public IActionResult ProfessionalCourses()
        {
            SetDateOfNextClassToSession();
            return View();
        }

        public IActionResult AcquireSkill()
        {
            SetDateOfNextClassToSession();
            return View();
        }

        public ActionResult GetProfessionalCV()
        {
            SetDateOfNextClassToSession();
            return View();
        }

        public IActionResult HRM()
        {
            SetDateOfNextClassToSession();
            return View();
        }

        public IActionResult ProjectManagement()
        {
            SetDateOfNextClassToSession();
            return View();
        }

        public IActionResult CustServiceMgt()
        {
            SetDateOfNextClassToSession();
            return View();
        }


        private void SetDateOfNextClassToSession()
        {
            // Check if session is already set otherwise set the date of next class to session
            if (HttpContext.Session.GetString("_dateOfNextClass") == null)
            {
                var nextClass = JsonConvert.SerializeObject(DateTime.UtcNow.Next(DayOfWeek.Sunday)).ToString();
                HttpContext.Session.SetString("_dateOfNextClass", nextClass);
            }
        }

    }
}
