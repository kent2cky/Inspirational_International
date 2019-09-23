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
using Microsoft.AspNetCore.Http;

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
        [HttpGet]
        public async Task<IActionResult> SubmitRSVP([FromBody]RSVPViewModel model = null)
        {
            _logger.LogCritical("RSVPSubmit Hit!!!!\n\n\n\n\n" +
             model.RSVP + model.PhoneNumber + model.FirstName + model.PictureData);



            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(User.Identity.Name);
                var date = DateTime.Now;
                var dateOfSunday = date.Next(DayOfWeek.Sunday);
                var expireTime = new DateTime(
                    dateOfSunday.Year,
                    dateOfSunday.Month,
                    dateOfSunday.Day,
                    23, 59, 59);
                Console.WriteLine("\n\n\n\n\n\n\n\n\n\n\n\n" + expireTime + "\n\n\n\n\n\n\n\n\n\n\n\n");
                // // // HttpContext.Response.Cookies.Append("_rsvp", "True",
                // // //         new CookieOptions()
                // // //         {
                // // //             Path = "/",
                // // //             Expires = expireTime,
                // // //             HttpOnly = true
                // // //         });
                // // var v = await _rsvpRepo.SumbitRSVPAsync(dateOfSunday, user.Id.ToString(), 0);

                // if (v != 0)
                // {
                //     _logger.LogError("RSVP submission not successful!!\n\n\n\n\n\n\n\n");
                //     //throw()
                // }
                _logger.LogInformation($"RSVP submitted successfully!!!\n\n\n\n\n\n\n\n");
                string response = "RSVP submitted!!";
                return Ok(response);
            }


            return RedirectToAction("Index", "Home");//BadRequest();//model);

        }

        [HttpGet]
        public async Task<IActionResult> SubmitRSVPs([FromQuery]string RSVP = null)
        {
            _logger.LogCritical("RSVPSubmit Hit!!!!\n\n\n\n\n" + RSVP);



            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(User.Identity.Name);
                var date = DateTime.Now;
                var dateOfSunday = date.Next(DayOfWeek.Sunday);
                var expireTime = new DateTime(
                    dateOfSunday.Year,
                    dateOfSunday.Month,
                    dateOfSunday.Day,
                    23, 59, 59);
                Console.WriteLine("\n\n\n\n\n\n\n\n\n\n\n\n" + expireTime + "\n\n\n\n\n\n\n\n\n\n\n\n");
                // HttpContext.Response.Cookies.Append("_rsvp", "True",
                //         new CookieOptions()
                //         {
                //             Path = "/",
                //             Expires = expireTime,
                //             HttpOnly = true
                //         });

                // var v = await _rsvpRepo.SumbitRSVPAsync(dateOfSunday, user.Id.ToString(), 0);

                // if (v != 0)
                // {
                //     _logger.LogError("RSVP submission not successful!!\n\n\n\n\n\n\n\n");
                //     //throw()
                // }

                return RedirectToAction("Index", "Home");
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
