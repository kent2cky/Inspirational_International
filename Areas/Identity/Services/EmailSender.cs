using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using SendGrid;
using SendGrid.Helpers.Mail;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Inspiration_International.Services
{
    public class EmailSender : IEmailSender
    {
        public EmailSender(IConfiguration configuration, ILogger<EmailSender> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }
        private IConfiguration _configuration { get; set; }
        private ILogger _logger { get; set; }
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var apiKey = _configuration["Secrets:SendGridApiKey"] ?? "Not found!!";
            Console.WriteLine(apiKey);
            await Execute(apiKey, email, subject, message);
        }

        static async Task Execute(string apiKey, string email, string subject, string message)
        {
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("kent2ckymaduka@gmail.com", "Kennis Maduka"),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            msg.AddTo(new EmailAddress(email));

            // Disable click tracking.
            // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
            msg.SetClickTracking(false, false);

            await client.SendEmailAsync(msg);

        }

    }
}