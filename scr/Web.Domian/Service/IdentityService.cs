using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Domain.Service.Abstractions;
using Web.Entity.Entity.Identity;

namespace Web.Domain.Service
{
    public class IdentityService : IIdentityService
    {

        private readonly UserManager<Users> userManager;
        private readonly SignInManager<Users> signInManager;
        private readonly RoleManager<Roles> roleManager;
        public IdentityService(IServiceScopeFactory _serviceScopeFactory, 
            SignInManager<Users> _signInManager,
            UserManager<Users> _userManager, 
            RoleManager<Roles> _roleManager
            )
        {
            this.userManager = _userManager;
            this.roleManager = _roleManager;
            this.signInManager = _signInManager;
        }

        public UserManager<Users> UserManager => userManager;

        public RoleManager<Roles> RoleManager => roleManager;

        public SignInManager<Users> SignInManager => signInManager;
    }
}
