using Web.Domain.Models.Account;

namespace Cohere.Domain.Models.Account
{
    public class RestorePasswordViewModel : EmailConfirmationViewModel
    {
        public string NewPassword { get; set; }
    }
}
