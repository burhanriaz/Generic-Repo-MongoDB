using System.ComponentModel.DataAnnotations;
using Web.Entity.Entity.Identity;
using Web.Domain.Models.Account;
using Web.Domain.Service.Abstractions;
using FluentValidation;
using Web.Domain.Models.ModelsAuxiliary;
using Web.Domain.Utils.Validators.User;
using Web.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Web.Api;
using Microsoft.AspNetCore.Authorization;
using Web.Api.ServiceCollectionsConfigurations.Authorization;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class AccountController : BaseController
    {
        private readonly IAccountService<UserViewModel, Users> accountService;
        private readonly IValidator<UserViewModel> userViewModelValidator;
        private readonly IValidator<RolesViewModel> roleViewModelValidator;
        private readonly IValidator<EmailConfirmationViewModel> emailConfirmationViewModelValidator;
        private readonly IValidator<ChangePasswordViewModel> changePasswordViewModelValidator;
        private readonly ILogger<AccountController> logger;

        public AccountController(
            IValidator<ChangePasswordViewModel> _changePasswordViewModelValidator,
            IValidator<EmailConfirmationViewModel> _emailConfirmationViewModelValidator,
            IValidator<RolesViewModel> _roleViewModelValidator,
            IValidator<UserViewModel> _userViewModelValidator,
            IAccountService<UserViewModel, Users> _accountService,
            ILogger<AccountController> _logger)
        {
            this.accountService = _accountService;
            this.userViewModelValidator = _userViewModelValidator;
            this.roleViewModelValidator = _roleViewModelValidator;
            this.emailConfirmationViewModelValidator = _emailConfirmationViewModelValidator;
            this.changePasswordViewModelValidator = _changePasswordViewModelValidator;
            this.logger = _logger;
        }
        [HttpPost("CreateRole")]
        [PermissionAuthorize(PermissionRoles.WriteRole)]
        public async Task<IActionResult> CreateRole(RolesViewModel rolesVm)
        {
            var validationResult = await roleViewModelValidator.ValidateAsync(rolesVm);
            if (validationResult.IsValid)
            {
                var result = await accountService.CreateRoleAsync(rolesVm);
                if (result.Succeeded)
                {
                    return Ok(result.Payload);
                }
                else
                {
                    return BadRequest(result);
                }

            }
            return BadRequest(new ErrorInfo { Message = validationResult.ToString() });
        }

        [HttpPut("UpdateRole")]
        [PermissionAuthorize(PermissionRoles.WriteRole)]
        public async Task<IActionResult> UpdateRole(RolesViewModel rolesVm)
        {
            var validationResult = await roleViewModelValidator.ValidateAsync(rolesVm);
            if (validationResult.IsValid)
            {
                var result = await accountService.UpdateRoleAsync(rolesVm);
                if (result.Succeeded)
                {
                    return Ok(result.Payload);
                }
                else
                {
                    return BadRequest(result);
                }
            }
            return BadRequest(new ErrorInfo { Message = validationResult.ToString() });
        }

        [HttpGet("GetAllRoles")]
        [PermissionAuthorize(PermissionRoles.ReadRole)]
        public async Task<IActionResult> GetAllRoles()
        {
            var result = await accountService.GetAllRolesAsync();
            if (result.Succeeded)
            {
                return Ok(result.Payload);
            }
            return BadRequest(result);

        }

        [HttpGet("GetRoleById")]
        [PermissionAuthorize(PermissionRoles.ReadRole)]
        public async Task<IActionResult> GetRoleById([Required] Guid roleId)
        {
            try
            {
                if (roleId != null)
                {
                    var result = await accountService.GetRoleByIdAsync(roleId);
                    if (result.Succeeded)
                    {
                        return Ok(result.Payload);
                    }
                    return BadRequest(result);
                }
                return BadRequest("Role Id can't be empty");

            }
            catch (Exception ex)
            {
                logger.LogError($"Exception occured during get role: {ex.Message}");
                throw;
            }

        }

        [HttpDelete("DeleteRoleById")]
        [PermissionAuthorize(PermissionRoles.DeleteRole)]
        public async Task<IActionResult> DeleteRoleById([Required] Guid roleId)
        {
            try
            {
                if (roleId != null)
                {
                    var result = await accountService.DeleteRoleByIdAsync(roleId);
                    if (result.Succeeded)
                    {
                        return Ok(result);
                    }
                    return BadRequest(result);
                }
                return BadRequest("Role Id can't be empty");

            }
            catch (Exception ex)
            {
                logger.LogError($"Exception occured during get role: {ex.Message}");
                throw;
            }

        }

        // the user who create role also access PermissionList
        [HttpGet("GetPermissionList")]
        [AllowAnonymous]
      //  [PermissionAuthorize(PermissionRoles.WriteRole)]
        public IEnumerable<ModulesPermission> GetPermissionList()
        {
            return PermissionDisplayList.ModulesPermissions;

        }

        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] UserViewModel userVm)
        {
            var validationResult = await userViewModelValidator.ValidateAsync(userVm);
            if (validationResult.IsValid)
            {
                var result = await accountService.CreateUserAsync(userVm);
                if (result.Succeeded)
                {
                    return Ok(result.Payload);
                }
                return BadRequest(result);
            }
            return BadRequest(new ErrorInfo { Message = validationResult.ToString() });
        }

        [HttpPut("UpdateUser")]

        public async Task<IActionResult> UpdateUser([FromBody] UserViewModel userVm)
        {
            var validator = new UserValidator(isUpdate: true);
            var validationResult = await validator.ValidateAsync(userVm);
            if (validationResult.IsValid)
            {
                var result = await accountService.UpdateUserAsync(userVm);
                if (result.Succeeded)
                {
                    return Ok(result.Payload);
                }
                return BadRequest(result);
            }
            return BadRequest(new ErrorInfo { Message = validationResult.ToString() });
        }

        [HttpGet("GetAllUsers")]
        [PermissionAuthorize(PermissionUsers.ReadUser)]
        public async Task<IActionResult> GetAllUsers()
        {

            var result = await accountService.GetAllUsersAsync();
            if (result.Succeeded)
            {
                return Ok(result.Payload);
            }
            return BadRequest(result);
        }

        [HttpGet("GetUserByEmail")]
        [PermissionAuthorize(PermissionUsers.ReadUser)]
        public async Task<IActionResult> GetUserByEmail([Required] string email)
        {
            var result = await accountService.GetUserByEmail(email);
            if (result.Succeeded)
            {
                return Ok(result.Payload);
            }
            return BadRequest(result);

        }

        [HttpGet("GetUserById")]
        [PermissionAuthorize(PermissionUsers.ReadUser)]
        public async Task<IActionResult> GetUserById([Required] Guid userId)
        {
            if (userId != null)
            {
                var result = await accountService.GetUserByIdAsync(userId);
                if (result.Succeeded)
                {
                    return Ok(result.Payload);
                }
                return BadRequest(result);

            }
            return BadRequest("User Id can't be empty");
        }

        [HttpDelete("DeleteUserById")]
        [PermissionAuthorize(PermissionUsers.DeleteUser)]
        public async Task<IActionResult> DeleteUserById([Required] Guid userId)
        {
            if (userId != null)
            {
                var result = await accountService.DeleteUserByIdAsync(userId);
                if (result.Succeeded)
                {
                    return Ok(result);
                }
                return BadRequest(result);

            }
            return BadRequest("User Id can't be empty");
        }

        //POST /Account/ConfirmEmail
        [HttpPost("ConfirmEmail")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail([FromBody] EmailConfirmationViewModel emailConfirmationModel)
        {

            if (emailConfirmationModel == null)
            {
                return BadRequest();
            }
            var validationResult = await emailConfirmationViewModelValidator.ValidateAsync(emailConfirmationModel);
            if (!validationResult.IsValid)
            {
                return BadRequest(new ErrorInfo { Message = validationResult.ToString() });
            }
            var result = await accountService.ConfirmEmailAsync(emailConfirmationModel);
            if (result.Succeeded)
            {
                return Ok(result.Message);
            }
            return BadRequest(result);

        }

        // GET: /Account/CheckEmailAvailability/email
        [HttpGet("CheckEmailAvailability/{email}")]
        [AllowAnonymous]
        public async Task<IActionResult> CheckEmailAvailability([EmailAddress] string email)
        {
            if (email == null)
            {
                return BadRequest(new ErrorInfo { Message = "Email cannot be null" });
            }
            var result = await accountService.IsEmailAvailableForRegistrationAsync(email);
            if (result.Succeeded)
            {
                return Ok(result);
            }
            return BadRequest(result);

        }
        // GET: /Account/UpdatePassword
        [HttpPost("UpdatePassword")]
        public async Task<IActionResult> UpdatePassword([FromBody] ChangePasswordViewModel changePasswordViewModel)
        {
            var validationResult = await changePasswordViewModelValidator.ValidateAsync(changePasswordViewModel);
            if (!validationResult.IsValid)
            {
                return BadRequest(new ErrorInfo { Message = validationResult.ToString() });
            }

            var result = await accountService.ChangePasswordAsync(changePasswordViewModel);
            if (result.Succeeded)
            {
                return Ok(result);
            }
            return BadRequest(result);

        }

      
    }
}
