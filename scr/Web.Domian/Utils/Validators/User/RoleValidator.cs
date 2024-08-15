using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Domain.Models.Account;

namespace Web.Domain.Utils.Validators.User
{
    public  class RoleValidator : AbstractValidator<RolesViewModel>
    {
        public RoleValidator()
        {
            RuleFor(u => u.Name).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty()
                .WithMessage("{PropertyName} should not be empty")
                .MinimumLength(3).WithMessage("Minimum length is {MinLength}")
                .MaximumLength(50).WithMessage("{PropertyName} maximum length is {MaxLength}");

            RuleFor(u => u.Permissions).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty()
                .WithMessage("{PropertyName} should not be empty at least add one permission");
        }
    }
    
}
