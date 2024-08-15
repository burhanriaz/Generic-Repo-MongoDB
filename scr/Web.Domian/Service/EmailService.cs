using Microsoft.Extensions.Logging;
using System.Net.Mail;
using System.Net;
using Web.Domain.Models;
using Web.Domain.Service.Abstractions;
using Microsoft.Extensions.Options;
using Web.Entity.Setting;

namespace Web.Domain.Service
{
    public class EmailService : IEmailService
    {
        private readonly ILogger<EmailService> logger;
        private readonly SMTPNetworkCredential credential;
        public EmailService(IOptions<SMTPNetworkCredential> _SMTPNetworkCredential, ILogger<EmailService> _logger)
        {
            this.credential = new SMTPNetworkCredential
            {
                Username = _SMTPNetworkCredential.Value.Username,
                Password = _SMTPNetworkCredential.Value.Password,
                DeliveryMethod = _SMTPNetworkCredential.Value.DeliveryMethod,
                EnableSsl = _SMTPNetworkCredential.Value.EnableSsl,
                Host = _SMTPNetworkCredential.Value.Host,
                Port = _SMTPNetworkCredential.Value.Port,
                UseDefaultCredentials = _SMTPNetworkCredential.Value.UseDefaultCredentials
            };
            logger = _logger;
        }
        private async Task SendAsync(EmailRequest email)
        {
            await Task.Run(() =>
            {
                SendEmail(email);
            });
        }
        private void Send(EmailRequest email)
        {
            var task = Task.Run(() =>
            {
                SendEmail(email);
            });
            task.Wait(); // Blocks the thread until the Task completes
        }
        private void SendEmail(EmailRequest email)
        {
            try
            {
                var message = new MailMessage
                {
                    From = new MailAddress(email.From, email.DisplayName),
                    Subject = email.Subject,
                    Body = email.Content,
                    IsBodyHtml = email.IsContentHtml
                };

                foreach (var receiver in email.To)
                {
                    message.To.Add(new MailAddress(receiver));
                }

                if (email.Attachments != null)
                {
                    foreach (var attachment in email.Attachments)
                    {
                        message.Attachments.Add(attachment);
                    }
                }
                using (var smtp = new SmtpClient())
                {
                    smtp.Host = credential.Host;
                    smtp.Port = credential.Port;
                    smtp.EnableSsl = credential.EnableSsl;
                    smtp.DeliveryMethod = (SmtpDeliveryMethod)credential.DeliveryMethod;
                    smtp.UseDefaultCredentials = credential.UseDefaultCredentials;
                    smtp.Credentials = new NetworkCredential(credential.Username, credential.Password);
                    try
                    {

                        smtp.Send(message);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "Error during sending notification");
                    }
                };

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error during sending notification in preparing Mail Message");
            }
        }

        public async Task SendAsync(string receiverAddress, string subject, string htmlContent)
        {
            await SendAsync(new EmailRequest(new List<string> { receiverAddress }, subject, htmlContent, true));

        }
        public async Task SendAsync(List<string> receiverAddresses, string subject, string htmlContent)
        {
            await SendAsync(new EmailRequest(receiverAddresses, subject, htmlContent, true));

        }
        public void Send(string receiverAddress, string subject, string htmlContent)
        {
            Send(new EmailRequest(new List<string> { receiverAddress }, subject, htmlContent, true));

        }
        public void Send(List<string> receiverAddresses, string subject, string htmlContent)
        {

            Send(new EmailRequest(receiverAddresses, subject, htmlContent, true));

        }
        public async Task SendWithAttachmentsAsysnc(List<string> receiverAddresses, string subject, string htmlContent, AttachmentCollection attachments)
        {
            await SendAsync(new EmailRequest(receiverAddresses, subject, htmlContent, true, attachments));
        }
    }
}
