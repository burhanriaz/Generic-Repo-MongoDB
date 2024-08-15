using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Domain.Models.Account;

namespace Web.Domain.Utils.Validators
{
    public class LoginValidator :  AbstractValidator<LoginViewModel>
    {
        public LoginValidator()
        {
            RuleFor(u => u.Email).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty()
                .WithMessage("{PropertyName} should not be empty");

            RuleFor(u => u.Password).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty()
                .WithMessage("{PropertyName} should not be empty");
        }
    }
}
