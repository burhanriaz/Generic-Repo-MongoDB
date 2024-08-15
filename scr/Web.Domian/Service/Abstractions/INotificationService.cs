namespace Web.Domain.Service.Abstractions
{
    public interface INotificationService
    {
        Task<string> GetTemplateContent(string templatePath);

        Task SendEmailConfirmationLinkAsync(string userEmail, string emailConfirmationToken);

        Task SendPasswordResetLinkAsync(string userId, string userEmail, string passwordRestorationToken);

    }
}