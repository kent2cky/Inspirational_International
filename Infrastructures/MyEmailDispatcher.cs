using System;
using System.Text;
using System.Threading.Tasks;
using Inspiration_International.Helpers;
using Inspiration_International.Identity;
using Inspiration_International.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Inspiration_International.Services
{
    public class MyEmailDispatcher : IJob
    {
        public ILogger<MyEmailDispatcher> _logger { get; set; }
        public IEmailSender _emailSender { get; set; }
        public IRSVPRepo _rsvpRepo { get; set; }


        public MyEmailDispatcher()
        {

        }
        public MyEmailDispatcher(IEmailSender emailSender, ILogger<MyEmailDispatcher> logger, IRSVPRepo rsvpRepo)
        {
            _logger = logger;
            _emailSender = emailSender;
            _rsvpRepo = rsvpRepo;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                _logger.LogInformation($"Dispatching contacts of those who signed up for the class on {DateTime.Now.Next(DayOfWeek.Sunday)} to the admin.........");

                // JobKey key = context.JobDetail.Key;
                // JobDataMap dataMap = context.MergedJobDataMap;
                var dateFor = DateTime.Now.Next(DayOfWeek.Sunday); // Get date of next class

                var contacts = await _rsvpRepo.GetAllRSVPWithTheirContacts(Convert.ToDateTime("2019-10-13"));
                var subject = new StringBuilder($"RSVPs for the tomorrows class {DateTime.Now.Next(DayOfWeek.Sunday)}");
                var message = new StringBuilder("<div>These are the contacts of the people that signed up for tomorrows class</div>");
                foreach (var contact in contacts)
                {
                    message.AppendLine($"<div>Email Address: {contact.Item2} Phone Number {contact.Item3}</div>");
                }
                await _emailSender.SendEmailAsync("kent2ckymaduka@gmail.com", subject.ToString(), message.ToString());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something went wrong in EmailDispatcher's Execute method.");
            }
        }

    }
}