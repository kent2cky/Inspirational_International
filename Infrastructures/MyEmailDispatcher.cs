using System;
using System.Text;
using System.Threading.Tasks;
using Inspiration_International.Helpers;
using Inspiration_International.Identity;
using Inspiration_International.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;

namespace Inspiration_International.Services
{


    public class MyEmailDispatcher : IJob
    {
        private ILogger<MyEmailDispatcher> _logger { get; set; }
        private IEmailSender _emailSender { get; set; }
        private IRSVPRepo _rsvpRepo { get; set; }
        private IApplicationLifetime _applicationLifetime { get; }


        public MyEmailDispatcher()
        {

        }
        public MyEmailDispatcher(IEmailSender emailSender, ILogger<MyEmailDispatcher> logger, IRSVPRepo rsvpRepo, IApplicationLifetime applicationLifetime)
        {
            _logger = logger;
            _emailSender = emailSender;
            _rsvpRepo = rsvpRepo;
            _applicationLifetime = applicationLifetime;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {

                _logger.LogInformation($"Dispatching contacts of those who signed up for the class on {DateTime.Now.Next(DayOfWeek.Sunday)} to the admin.........");

                var dateFor = DateTime.Now.Next(DayOfWeek.Sunday); // Get date of next class

                var contacts = await _rsvpRepo.GetAllRSVPWithTheirContacts(Convert.ToDateTime("2019-10-13"));
                var subject = new StringBuilder($"RSVPs for tomorrows class {DateTime.Now.Next(DayOfWeek.Sunday).ToString().Split(" ")[0]}");
                var message = new StringBuilder("<p>These are the contacts of the people that signed up for tomorrow's class:</p>");
                message.AppendLine("<table style=\"background: beige; \"><tr style=\"background: #098ee7e6\"><th>Email</th><th>Number</th></tr>");
                foreach (var contact in contacts)
                {
                    message.Append($"<tr><td>{contact.Item2}</td><td>{contact.Item3}</td></tr>");
                }
                message.Append("</table>");
                await _emailSender.SendEmailAsync("kent2ckymaduka@gmail.com", subject.ToString(), message.ToString());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something went wrong in EmailDispatcher's Execute method.");
            }
        }
    }
}