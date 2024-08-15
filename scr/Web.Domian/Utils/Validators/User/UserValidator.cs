using FluentValidation;
using Web.Domain.Models.Account;

namespace Web.Domain.Utils.Validators.User
{
    public class UserValidator : AbstractValidator<UserViewModel>
    {
        public UserValidator(bool isUpdate = false)
        {
            //RuleFor(u => u.Id).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty()
            //    .WithMessage("{PropertyName} not empty")
            //    .MaximumLength(150).WithMessage("{PropertyName} maximum length is {MaxLength}");


            RuleFor(u => u.FirstName).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty()
                .WithMessage("{PropertyName} should not be empty")
                .MinimumLength(1).WithMessage("Minimum length is {MinLength}")
                .MaximumLength(255).WithMessage("{PropertyName} maximum length is {MaxLength}");

            RuleFor(u => u.LastName).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty()
                .WithMessage("{PropertyName} should not be empty")
                .MinimumLength(1).WithMessage("Minimum length is {MinLength}")
                .MaximumLength(255).WithMessage("{PropertyName} maximum length is {MaxLength}");

            RuleFor(a => a.Email).Cascade(CascadeMode.StopOnFirstFailure).NotNull().WithMessage("{PropertyName} is required")
                .EmailAddress().WithMessage("Email address")
                .MinimumLength(3).WithMessage("{PropertyName} min length is {MinLength}")
                .MaximumLength(100).WithMessage("{PropertyName} max length is {MaxLength}");

            RuleFor(a => a.Email)
          .Cascade(CascadeMode.StopOnFirstFailure)
          .NotNull().WithMessage("{PropertyName} is required.")
          .EmailAddress().WithMessage("Invalid email address format.")
          .MinimumLength(3).WithMessage("{PropertyName} must be at least {MinLength} characters long.")
          .MaximumLength(100).WithMessage("{PropertyName} cannot exceed {MaxLength} characters.");

            //RuleFor(a => a.Password)
            // .Cascade(CascadeMode.StopOnFirstFailure)
            // .NotNull().WithMessage("Password is required.")
            // .Matches("^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.{8,})")
            // .WithMessage("Password must contain at least 1 lowercase alphabetical character, at least 1 uppercase alphabetical character, at least 1 numeric character, and must be eight characters or longer.")
            // .MaximumLength(250).WithMessage("Password length must not exceed 250 characters.");

            //RuleFor(a => a.ConfirmPassword)
            //    .Cascade(CascadeMode.StopOnFirstFailure)
            //    .NotNull().WithMessage("Confirm Password is required.")
            //    .Equal(a => a.Password)
            //    .WithMessage("Confirm Password must match the Password.");

            When(u => u.PhoneNumber != null, () =>
            {
                RuleFor(u => u.PhoneNumber).MaximumLength(20)
                    .WithMessage("{PropertyName} maximum length is {MaxLength}");
            });
            // Validate Password and ConfirmPassword only if they are not null during an update
            When(u => isUpdate, () => { RuleFor(u => u.Password).Empty(); });
            When(u => isUpdate, () => { RuleFor(u => u.ConfirmPassword).Empty(); });

            When(u => !isUpdate, () =>
            {
                // Validation rules for Password and ConfirmPassword when not null are applied for non-update scenarios
                RuleFor(u => u.Password)
                    .Cascade(CascadeMode.StopOnFirstFailure)
                    .NotNull().WithMessage("Password is required.")
                    .Matches("^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.{8,})")
                    .WithMessage("Password must contain at least 1 lowercase alphabetical character, at least 1 uppercase alphabetical character, at least 1 numeric character, and must be eight characters or longer.")
                    .MaximumLength(250).WithMessage("Password length must not exceed 250 characters.");

                RuleFor(u => u.ConfirmPassword)
                    .Cascade(CascadeMode.StopOnFirstFailure)
                    .NotNull().WithMessage("Confirm Password is required.")
                    .Equal(u => u.Password)
                    .WithMessage("Confirm Password must match the Password.");
            });
        }
    }
}
