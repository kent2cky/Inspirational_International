using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Inspiration_International.Repositories;
using Inspiration_International.Models;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Inspiration_International.Identity;
using Inspiration_International.Helpers;
using Microsoft.AspNetCore.Http;
using System.Globalization;

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
        public IActionResult Index()
        {
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> SubmitRSVP()
        {
            _logger.LogInformation($"Submitting RSVP from user: {User.Identity.Name}...............\n");

            if (ModelState.IsValid)
            {
                // Get the ID of the user
                var user = await _userManager.FindByEmailAsync(User.Identity.Name);

                // Next(DayOfWeek.Sunday) is an extension method.
                var dateOfNextClass = DateTime.Now.Next(DayOfWeek.Sunday);

                // Returns 0 if success else 1
                var response = await _rsvpRepo.SubmitRSVPAsync(dateOfNextClass, user.Id.ToString(), 0);

                if (response == 0)
                {
                    _logger.LogInformation($"RSVP submitted successfully!!!\n");
                    return Ok("RSVP submitted!");
                }

                _logger.LogError("RSVP submission not successful!!\n");

                return BadRequest("RSVP submission not successful. Try again later.");
            }
            // If it made it here then something went wrong
            return BadRequest("Bad Request.");
        }

        [HttpGet]
        public async Task<IActionResult> SubmitRSVPs([FromQuery]string RSVP = null)
        {
            _logger.LogInformation($"Submitting RSVP from user: {User.Identity.Name}...............\n");

            if (ModelState.IsValid)
            {
                // To get id of the user
                var user = await _userManager.FindByEmailAsync(User.Identity.Name);
                var dateOfNextClass = DateTime.Now.Next(DayOfWeek.Sunday);
                var expireTime = new DateTime(
                    dateOfNextClass.Year,
                    dateOfNextClass.Month,
                    dateOfNextClass.Day,
                    23, 59, 59);

                var response = await _rsvpRepo.SubmitRSVPAsync(dateOfNextClass, user.Id.ToString(), 0);

                if (response == 0)
                {
                    _logger.LogInformation($"RSVP submitted successfully!!!\n");

                    // Redirect to home so as to refresh the page.
                    return RedirectToAction("Index", "Home");
                }

                _logger.LogError("Error submitting RSVP!!\n");

                return BadRequest("Error submitting RSVP!! Try again later.");
            }

            return BadRequest("Bad Request.");
        }

        [HttpPost]
        public async Task<IActionResult> saveUserPhoneNumber([FromBody] PhoneNumberModel phoneNumberModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    UInt64 number;

                    if (!UInt64.TryParse(phoneNumberModel.PhoneNumber, System.Globalization.NumberStyles.AllowLeadingSign,
                                        CultureInfo.InvariantCulture, out number))
                    {
                        _logger.LogError($"Invalid value supplied as phone number: {phoneNumberModel.PhoneNumber}");
                        return BadRequest($"Invalid value supplied as phone number.");
                    }
                    _logger.LogInformation($"Saving user {User.Identity.Name}'s phone number...............\n");
                    var user = await _userManager.FindByNameAsync(User.Identity.Name);
                    user.PhoneNumber = phoneNumberModel.PhoneNumber;
                    await _userManager.UpdateAsync(user);
                    _logger.LogInformation($"User {user.Email}'s phone number saved.......");
                    return Ok($"Phone number saved.......");
                }
                _logger.LogError($"Invalid value supplied as phone number: {phoneNumberModel.PhoneNumber}.");
                return BadRequest($"Invalid value supplied as phone number.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something went wrong in HomeController's saveUserPhoneNumber endpoint.");
                return BadRequest();
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
