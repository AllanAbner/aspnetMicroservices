using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Models;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Mail
{
    public class EmailService : IEmailService
    {
        public EmailSettings emailSettings;
        private readonly ILogger<EmailService> logger;

        public EmailService(IOptions<EmailSettings> emailSettings, ILogger<EmailService> logger)
        {
            this.emailSettings = emailSettings?.Value ?? throw new System.ArgumentNullException(nameof(emailSettings));
            this.logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
        }

        public async Task<bool> SendEmail(Email email)
        {
            var client = new SendGridClient(emailSettings.ApiKey);

            var from = new EmailAddress(emailSettings.FromAddress, emailSettings.FromName);
            var subject = "Sending with SendGrid is Fun";
            var to = new EmailAddress(email.To);
            var plainTextContent = "and easy to do anywhere, even with C#";
            var htmlContent = "<strong>and easy to do anywhere, even with C#</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);

            logger.LogInformation("Email Sent");

            return response.IsSuccessStatusCode;
        }
    }
}