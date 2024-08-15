using FluentValidation;
using Web.Domain.Models.Account;

namespace Web.Domain.Utils.Validators.User
{
    public class ChangePasswordValidator : AbstractValidator<ChangePasswordViewModel>
    {
        public ChangePasswordValidator()
        {
            RuleFor(a => a.Email).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty()
               .EmailAddress().WithMessage("{PropertyName} should not be empty");

            RuleFor(u => u.CurrentPassword)
           .Cascade(CascadeMode.StopOnFirstFailure)
           .NotNull().WithMessage("Password is required.");
          
            RuleFor(u => u.NewPassword)
                    .Cascade(CascadeMode.StopOnFirstFailure)
                    .NotNull().WithMessage("Password is required.")
                    .Matches("^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.{8,})")
                    .WithMessage("Password must contain at least 1 lowercase alphabetical character, at least 1 uppercase alphabetical character, at least 1 numeric character, and must be eight characters or longer.")
                    .MaximumLength(250).WithMessage("Password length must not exceed 250 characters.");

            RuleFor(u => u.ConfirmPassword)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull().WithMessage("Confirm Password is required.")
                .Equal(u => u.NewPassword)
                .WithMessage("Confirm Password must match the Password.");

        }
    }
}
