using System.Dynamic;
using System.Web;
using AutoMapper;
using Newtonsoft.Json;
using Web.Entity.UnitOfWork;
using Web.Domain;
using Microsoft.Extensions.Logging;
using Web.Domain.Service.Abstractions;
using Web.Domain.Utils;
using System.Reflection;

namespace Web.Domain.Service
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly ILogger<NotificationService> _logger;
        private readonly IEmailService emailService;

        private readonly string emailVerificationLink;
        private readonly string passwordRestorationRedirectUrl;

        public const string EmailVerificationLink = "EmailVerificationLink";
        public const string PasswordRestorationRedirectUrl = "PasswordRestorationRedirectUrl";


        public NotificationService(
            IUnitOfWork _unitOfWork,
            IMapper _mapper,
            IEmailService _emailService,
            ILogger<NotificationService> logger,
            Func<string, string> _emailVerificationLinkResolver,
            Func<string, string> _passwordRestorationRedirectUrlResolver
         ) 
        {
            unitOfWork =  _unitOfWork ;
            mapper = _mapper;
            emailService = _emailService;
            _logger = logger;
            emailVerificationLink = _emailVerificationLinkResolver.Invoke(EmailVerificationLink);
            passwordRestorationRedirectUrl = _passwordRestorationRedirectUrlResolver.Invoke(PasswordRestorationRedirectUrl);

        }
        public string GetTemplateFullPath(string templatePath)
        {
          return Path.Combine(AppContext.BaseDirectory, templatePath);
        }
        public async Task<string> GetTemplateContent(string templatePath)
        {
            return await File.ReadAllTextAsync(GetTemplateFullPath(templatePath));
        }
        public async Task SendEmailConfirmationLinkAsync(string userEmail, string emailConfirmationToken)
        {
            var emailHtmlTemplate = await GetTemplateContent(Constants.EmailTemplates.Account.EmailConfirmation);
          
            var uriBuilder = new UriBuilder(emailVerificationLink);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["email"] = userEmail;
            query["token"] = emailConfirmationToken;
            uriBuilder.Query = query.ToString();

            var personalizedHtmlTemplate = emailHtmlTemplate
                .Replace("{verifyEmailLink}", uriBuilder.ToString()
                .Replace("{year}", DateTime.UtcNow.Year.ToString()));

            await emailService.SendAsync(userEmail, "Welcome to Web Project. Please Verify Your Email",
                personalizedHtmlTemplate);
        }

        public async Task SendPasswordResetLinkAsync(string accountId, string reciverEmail, string passwordRestorationToken)
        {
            //var user = await _unitOfWork.GetRepositoryAsync<User>().GetOne(u => u.AccountId == accountId);

            //var uriBuilder = new UriBuilder(_passwordRestorationRedirectUrl);
            //var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            //query["email"] = accountEmail;
            //query["token"] = passwordRestorationToken;
            //uriBuilder.Query = query.ToString();

            //var emailHtmlTemplate = await GetTemplateContent(Constants.TemplatesPaths.Account.PasswordResetEmail);

            //var personalizedHtmlTemplate = emailHtmlTemplate
            //    .Replace("{userFirstName}", user.FirstName)
            //    .Replace("{passwordResetLink}", uriBuilder.ToString())
            //    .Replace("{unsubscribeEmailLink", _unsubscribeEmailLink)
            //    .Replace("{year}", DateTime.UtcNow.Year.ToString());

            //await _emailService.SendAsync(accountEmail, "Reset Your Password", personalizedHtmlTemplate);
        }

       
    }
}
