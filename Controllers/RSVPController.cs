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
using Microsoft.Extensions.Logging;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Inspiration_International.Identity;
using Inspiration_International.Helpers;

namespace Inspiration_International.Controllers
{
    [Authorize]
    public class RSVPController : Controller
    {
        public IRSVPRepo _rsvpRepo { get; set; }
        public ILogger<RSVPController> _logger { get; set; }
        public UserManager<ApplicationUser> _userManager { get; set; }


        public RSVPController(IRSVPRepo rsvpRepo, ILogger<RSVPController> logger,
                                                UserManager<ApplicationUser> userManager)
        {
            _rsvpRepo = rsvpRepo;
            _logger = logger;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<JsonResult> SubmitRSVP([FromBody]RSVPViewModel model)
        {
            _logger.LogCritical("RSVPSubmit Hit!!!!\n\n\n\n\n");

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(User.Identity.Name);
                var date = DateTime.Now;
                var dateOfSunday = date.Next(DayOfWeek.Sunday);
                var v = await _rsvpRepo.SumbitRSVPAsync(dateOfSunday, user.PhoneNumber, user.FullName, 0);

                if (v != 0)
                {
                    _logger.LogError("RSVP submission not successful!!\n\n\n\n\n\n\n\n");
                    return Json("Error");
                }
                _logger.LogInformation($"RSVP submitted successfully!!!\n\n\n\n\n\n\n\n");
                return Json("Success");
            }


            return Json("Index");

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
