using Cohere.Domain.Models.Account;
using FluentValidation;
using Web.Domain.Models.Account;

namespace Web.Domain.Utils.Validators.User
{
    public class EmailConfirmationValidator : AbstractValidator<EmailConfirmationViewModel>
    {
        public EmailConfirmationValidator()
        {
            RuleFor(a => a.Email).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty()
                .EmailAddress().WithMessage("{PropertyName} should not be empty");

            RuleFor(u => u.Token).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty()
                .WithMessage("{PropertyName} should not be empty");
        }
    }
}
