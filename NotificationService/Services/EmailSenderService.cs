using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NotificationService.Models;
using System.Net;
using System.Net.Mail;

namespace NotificationService.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string to, string subject, string body);
    }

    public class EmailSenderService : IEmailSender
    {
        private readonly ILogger<EmailSenderService> _logger;
        private readonly SmtpSettings _smtpSettings;

        public EmailSenderService(
            ILogger<EmailSenderService> logger,
            IOptions<SmtpSettings> smtpOptions)
        {
            _logger = logger;
            _smtpSettings = smtpOptions.Value;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var message = new MailMessage
            {
                From = new MailAddress(_smtpSettings.Username),
                Subject = subject,
                Body = body,
                IsBodyHtml = false
            };
            message.To.Add(to);

            using var smtp = new SmtpClient(_smtpSettings.Host, _smtpSettings.Port)
            {
                Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password),
                EnableSsl = true
            };

            try
            {
                await smtp.SendMailAsync(message);
                _logger.LogInformation($"📧 Email sent to {to}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"❌ Email sending failed: {ex.Message}");
            }
        }
    }
}
