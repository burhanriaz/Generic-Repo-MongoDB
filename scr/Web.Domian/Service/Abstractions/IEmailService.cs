using System.Net.Mail;

namespace Web.Domain.Service.Abstractions
{
    public interface IEmailService
    {
        Task SendAsync(string receiverAddress, string subject, string htmlContent);

        Task SendAsync(List<string> receiverAddresses, string subject, string htmlContent);

        void Send(string receiverAddress, string subject, string htmlContent);

        void Send(List<string> receiverAddresses, string subject, string htmlContent);

        Task SendWithAttachmentsAsysnc(List<string> receiverAddresses, string subject, string htmlContent, AttachmentCollection attachments);
    }
}
