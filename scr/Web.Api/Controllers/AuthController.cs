using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Domain.Models.Account;
using Web.Domain.Models.ModelsAuxiliary;
using Web.Domain.Service.Abstractions;
using Web.Entity.Entity.Identity;
using Web.Api.Utils.JWTTokenGenerator;

namespace Web.Api.Controllers
{
    //[Route("api/v{version:apiVersion}/[controller]")]
    [Route("[controller]")]
    [ApiController]
    public class AuthController : BaseController
    {
        private readonly IAuthService authService;
        private readonly ITokenGenerator tokenGenerator;
        private readonly IValidator<LoginViewModel> loginValidator;
        private readonly ILogger<AuthController> logger;
        private readonly IMapper mapper;
        public AuthController(
            IAuthService _authService,
            ITokenGenerator _tokenGenerator,
            IValidator<LoginViewModel> _loginValidator,
            IMapper _mapper,
             ILogger<AuthController> _logger
            ) : base(_tokenGenerator)
        {
            authService = _authService;
            tokenGenerator = _tokenGenerator;
            loginValidator = _loginValidator;
            mapper = _mapper;
            logger = _logger;
        }

        // POST: /Auth/SignIn
        [AllowAnonymous]
        [HttpPost("SignIn")]
        public async Task<IActionResult> SignIn([FromBody] LoginViewModel loginVm)
        {
            try
            {
                var validationResult = await loginValidator.ValidateAsync(loginVm);
                if (!validationResult.IsValid)
                {
                    return BadRequest(new ErrorInfo { Message = validationResult.ToString() });
                }

                IActionResult response;
                var result = await authService.SignInAsync(loginVm, false);

                if (result.Succeeded)
                {
                    var userVm = (UserViewModel)result.Payload;
                    AddOAuthTokenToResponseHeader(userVm);
                    response = Ok(userVm);
                }
                else
                {
                    response = BadRequest(new ErrorInfo { Message = result.Message });
                }

                return response;
            }
            catch (Exception ex)
            {
                logger.LogError($"Exception occured during login {loginVm.Email}: {ex.Message}");
                throw;
            }
        }

        //[Authorize]
        //[HttpGet("GetAccountInfo")]
        //public async Task<IActionResult> GetAccountInfo()
        //{
        //    var accountData = await _authService.GetUserData(AccountId);

        //    return accountData.ToActionResult();
        //}
    }
}