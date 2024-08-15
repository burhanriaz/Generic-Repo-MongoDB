using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Entity.Entity.Identity;

namespace Web.Domain.Service.Abstractions
{
    public interface IIdentityService
    {
        public UserManager<Users> UserManager { get; }
        public RoleManager<Roles> RoleManager { get; }
        public SignInManager<Users> SignInManager { get; }
       
    }

}
