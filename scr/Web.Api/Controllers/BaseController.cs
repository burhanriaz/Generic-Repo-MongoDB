using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Web.Domain.Models.Account;
using Web.Entity.Entity.Identity;
using Microsoft.AspNetCore.Authorization;
using Web.Api.Utils.JWTTokenGenerator;

namespace Web.Api.Controllers
{
    public class BaseController : ControllerBase
    {
        private readonly ITokenGenerator _tokenGenerator;
        public BaseController(ITokenGenerator tokenGenerator = null)
        {
            _tokenGenerator = tokenGenerator;
        }
        public string UserId => User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        public string AccountEmail => User.FindFirst(ClaimTypes.Email)?.Value;

        protected void AddOAuthTokenToResponseHeader(UserViewModel model)
        {
            HttpContext.Response.Headers.AccessControlExposeHeaders = "o-auth-token";
            HttpContext.Response.Headers.Add("o-auth-token", _tokenGenerator?.GenerateToken(model));
        }
    }
}

