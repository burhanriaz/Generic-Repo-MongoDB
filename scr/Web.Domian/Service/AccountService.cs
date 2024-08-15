using AutoMapper;
using Web.Entity.Entity.Identity;
using Web.Domain.Models.Account;
using Web.Entity.UnitOfWork;
using Web.Domain.Service.Abstractions;
using Web.Domain.Service.Generic;
using Web.Domain.Infrastructure;
using System.Data;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Identity;
using Web.Domain.Utils.Validators.User;
using Web.Entity.Entity;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using AspNetCore.Identity.MongoDbCore.Models;
using Microsoft.VisualBasic;

namespace Web.Domain.Service
{
    public class AccountService<TViewModel, TEntity> : GenericServiceAsync<TViewModel, TEntity>,
        IAccountService<TViewModel, TEntity> where TViewModel : UserViewModel where TEntity : Users
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IIdentityService identityService;
        private readonly IMapper mapper;
        private readonly INotificationService notificationService;
        private readonly ILogger<AccountService<TViewModel, TEntity>> logger;
        private readonly UserManager<Users> userManager;
        private readonly RoleManager<Roles> roleManager;

        public AccountService(ILogger<AccountService<TViewModel, TEntity>> _logger,
            INotificationService _notificationService,
            IUnitOfWork _unitOfWork,
            IIdentityService _identityService,
            IMapper _mapper) : base(_unitOfWork, _mapper)
        {
            this.unitOfWork = _unitOfWork;
            this.identityService = _identityService;
            this.mapper = _mapper;
            this.notificationService = _notificationService;
            this.logger = _logger;
            userManager = identityService.UserManager;
            roleManager = identityService.RoleManager;
            
        }
        public async Task<OperationResult> CreateUserAsync(UserViewModel userVm)
        {
            try
            {
                var existingUser = await userManager.FindByEmailAsync(userVm.Email);

                if (existingUser is not null)
                {
                    return OperationResult.Failure($"User already exists with following Email :{userVm.Email}");
                }

                var user = mapper.Map<Users>(userVm);
                var roles = await unitOfWork.GetRepositoryAsync<Roles>().Get(x => userVm.Roles.Any(userRole => x.Id == userRole.Id));

                if (roles.Any())
                {
                    user.Roles = roles.Select(x => x.Id).ToList();
                }
                // set create time and update time
                user.CreateTime = DateTime.UtcNow;
                user.UpdateTime = DateTime.UtcNow;
                user.EmailConfirmed = false;

                // user creating with usermanger 
                var result = await userManager.CreateAsync(user, userVm.Password);

                if (result.Succeeded)
                {
                    var userInserted = await unitOfWork.GetRepositoryAsync<Users>().GetOne(x => x.Email == userVm.Email);
                    var emailConfirmationToken = await userManager.GenerateEmailConfirmationTokenAsync(user);

                    await notificationService.SendEmailConfirmationLinkAsync(userInserted.Email, emailConfirmationToken);

                    var userVMAfterInsert = mapper.Map<UserViewModel>(userInserted);
                    return OperationResult.Success("User has been registered", userVMAfterInsert);
                }
                else
                {
                    return OperationResult.Failure(result.Errors.ToList().ToString(), userVm);

                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Exception occured during user creation in service: {ex.Message}");
                throw;
            }
        }
        public async Task<OperationResult> UpdateUserAsync(UserViewModel userVm)
        {
            try
            {

                var existingUser = await userManager.FindByIdAsync(userVm.Id.ToString());

                if (existingUser is null)
                {
                    return OperationResult.Failure($"User was not exists with following ID :{userVm.Id}");
                }

                //var user = mapper.Map<Users>(userVm);
                var roles = await unitOfWork.GetRepositoryAsync<Roles>().Get(x => userVm.Roles.Any(userRole => x.Id == userRole.Id));

                if (roles.Any())
                {
                    foreach (var role in roles)
                    {
                        if (!existingUser.Roles.Contains(role.Id))
                        {
                            existingUser.Roles.Add(role.Id);
                        }
                    }
                }
                // Remove roles that are not in the roles fetched from the database
                existingUser.Roles.RemoveAll(roleId => !roles.Any(role => role.Id == roleId));
                // set only update time
                existingUser.UpdateTime = DateTime.UtcNow;
                existingUser.FirstName = userVm.FirstName;
                existingUser.LastName = userVm.LastName;


                // user Update with usermanger 
                var result = await userManager.UpdateAsync(existingUser);

                if (result.Succeeded)
                {
                    var userInserted = await unitOfWork.GetRepositoryAsync<Users>().GetOne(x => x.Email == userVm.Email);
                    var userVMAfterInsert = mapper.Map<UserViewModel>(userInserted);
                    return OperationResult.Success("User has been updated", userVMAfterInsert);
                }
                else
                {
                    return OperationResult.Failure(result.Errors.ToList().ToString(), userVm);

                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Exception occured during user updation in service: {ex.Message}");
                throw;
            }
        }
        public async Task<OperationResult> GetAllUsersAsync()
        {
            try
            {
                var users = await unitOfWork.GetGenericRepositoryAsync<Users>().GetAll();
                var usersVm = Mapper.Map<List<Users>, List<UserViewModel>>(users.ToList());
                return OperationResult.Success(string.Empty, usersVm);
            }
            catch (Exception ex)
            {
                logger.LogError($"Exception occured during getting all user in service: {ex.Message}");
                throw;
            }
        }
        public async Task<OperationResult> GetUserByIdAsync(Guid userId)
        {
            try
            {
                var user = await unitOfWork.GetGenericRepositoryAsync<Users>().GetOne(u => u.Id == userId);
                if (user != null)
                {
                    var userVm = Mapper.Map<Users, UserViewModel>(user);
                    return OperationResult.Success(string.Empty, userVm);
                }
                return OperationResult.Failure("User not found");
            }
            catch (Exception ex)
            {
                logger.LogError($"Exception occured during get user in service: {ex.Message}");
                throw;
            }
        }
        public async Task<OperationResult> DeleteUserByIdAsync(Guid userId)
        {
            try
            {
                var user = await unitOfWork.GetGenericRepositoryAsync<Users>().GetOne(u => u.Id == userId);
                if (user != null)
                {
                    await unitOfWork.GetGenericRepositoryAsync<Users>().Delete(user.Id);
                    return OperationResult.Success("User Deleted successfully",user.Id);

                }
                return OperationResult.Failure("User not found");
            }
            catch (Exception ex)
            {
                logger.LogError($"Exception occured during get user in service: {ex.Message}");
                throw;
            }
        }
        public async Task<OperationResult> GetUserByEmail(string email)
        {
            try
            {
                var user = await unitOfWork.GetRepositoryAsync<Users>().GetOne(u => u.Email == email);
                if (user != null)
                {
                    var userVm = Mapper.Map<Users, UserViewModel>(user);
                    return OperationResult.Success(string.Empty, userVm);
                }
                return OperationResult.Failure($"User not found with this email:{email}");
            }
            catch (Exception ex)
            {
                logger.LogError($"Exception occured during get user in service: {ex.Message}");
                throw;
            }
        }
        public async Task<OperationResult> CreateRoleAsync(RolesViewModel roleVm)
        {
            try
            {
                // check if same name role already in db
                var checkExistingRoleWithName = await roleManager.RoleExistsAsync(roleVm.Name.ToLower());
                if (checkExistingRoleWithName)
                {
                    return OperationResult.Failure($"Role can't be create with the same name again :{roleVm.Name}");

                }
                if (roleVm.Permissions == null || !roleVm.Permissions.Any())
                {
                    return OperationResult.Failure($"Please Select At Least One Access : {roleVm.Name}");
                }
                //var role = mapper.Map<Roles>(roleVm);
                var role = new Roles
                {
                    CreateTime = DateTime.UtcNow,
                    UpdateTime = DateTime.UtcNow,
                    Name = roleVm.Name
                };
                // role creating with roleManager 
                var result = await roleManager.CreateAsync(role);

                if (result.Succeeded)
                {

                    foreach (var permission in roleVm.Permissions)
                    {
                        var claim = new Claim(ClaimsConstants.PermissionClaim, permission);

                        await roleManager.AddClaimAsync(role, claim);

                    }
                    var roleInserted = await _unitOfWork.GetRepositoryAsync<Roles>().GetOne(x => x.Name == roleVm.Name);
                    var roleVmAfterInsert = mapper.Map<RolesViewModel>(roleInserted);
                    return OperationResult.Success("Role has been created", roleVmAfterInsert);
                }
                else
                {
                    return OperationResult.Failure(result.Errors.ToList().ToString(), roleVm);
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Exception occured during role creation in service: {ex.Message}");
                throw;
            }
        }
        public async Task<OperationResult> UpdateRoleAsync(RolesViewModel roleVm)
        {
            try
            {

                var existingRole = await roleManager.FindByIdAsync(roleVm.Id.ToString());
                if (existingRole is null)
                {
                    return OperationResult.Failure($"Role does not exist with the following ID: {roleVm.Id}");
                }

                // Convert the role name to lowercase for comparison
                var newRoleNameLowercase = roleVm.Name.ToLower();

                // Check if a role with the same name (case insensitive) already exists in the database
                var checkExistingRoleWithName = await roleManager.FindByNameAsync(newRoleNameLowercase);
                if (checkExistingRoleWithName != null && checkExistingRoleWithName.Id != roleVm.Id)
                {
                    return OperationResult.Failure($"Role with the same name already exists: {roleVm.Name}");
                }
                //var role = mapper.Map<Roles>(roleVm);

                //Update the existing role's name and update time
                existingRole.Name = roleVm.Name;
                existingRole.UpdateTime = DateTime.UtcNow;

                 var result = await roleManager.UpdateAsync(existingRole);

                if (result.Succeeded)
                {
                    var existingClaims = await roleManager.GetClaimsAsync(existingRole);

                    //Adding the New Claims
                    foreach (var permission in roleVm.Permissions)
                    {
                        if (existingClaims.Any(x => x.Value == permission))
                        {
                            continue;
                        }
                        else
                        {
                            await roleManager.AddClaimAsync(existingRole, new Claim(ClaimsConstants.PermissionClaim, permission));
                        }

                    }
                    //Removing the Claims
                    var toBeRemoved = existingClaims.Where(x => !roleVm.Permissions.Contains(x.Value)).ToList();

                    foreach (var claim in toBeRemoved)
                    {
                        await roleManager.RemoveClaimAsync(existingRole, claim);
                    }

                    await roleManager.UpdateAsync(existingRole);
                    var roleVmAfterUpdate = mapper.Map<RolesViewModel>(existingRole);
                    return OperationResult.Success("Role has been updated", roleVmAfterUpdate);
                }
                else
                {
                    return OperationResult.Failure(result.Errors.FirstOrDefault()?.Description ?? "Role update failed", roleVm);
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Exception occured during role updation in service: {ex.Message}");
                throw;
            }
        }
        public async Task<OperationResult> GetAllRolesAsync()
        {
            try
            {
                var roles = await unitOfWork.GetGenericRepositoryAsync<Roles>().GetAll();
                var rolesVm = Mapper.Map<List<Roles>, List<RolesViewModel>>(roles.ToList());
                return OperationResult.Success(string.Empty, rolesVm);
            }
            catch (Exception ex)
            {
                logger.LogError($"Exception occured during getting roles in service: {ex.Message}");
                throw;
            }
        }
        public async Task<OperationResult> GetRoleByIdAsync(Guid roleId)
        {
            try
            {
                var role = await unitOfWork.GetGenericRepositoryAsync<Roles>().GetOne(u => u.Id == roleId);
                if (role != null)
                {
                    var roleVm = Mapper.Map<Roles, RolesViewModel>(role);
                    return OperationResult.Success(string.Empty, roleVm);
                }
                return OperationResult.Failure("Role not found");
            }
            catch (Exception ex)
            {
                logger.LogError($"Exception occured during role get in service: {ex.Message}");
                throw;
            }
        }
        public async Task<OperationResult> DeleteRoleByIdAsync(Guid roleId)
        {
            try
            {
                var role = await unitOfWork.GetGenericRepositoryAsync<Roles>().GetOne(u => u.Id == roleId);
                if (role != null)
                {
                    var users = await unitOfWork.GetGenericRepositoryAsync<Users>().Get(x=>x.Roles.Contains(role.Id));
                    if (users != null && users.Any())
                    {
                        return OperationResult.Failure("Role has been found by someone and cannot be deleted", role);

                    }
                    var isRoleDeleted = await unitOfWork.GetGenericRepositoryAsync<Roles>().Delete(role);
                    return OperationResult.Success("Role has been deleted successfully", role.Id);

                }
                return OperationResult.Failure("Role not found");
            }
            catch (Exception ex)
            {
                logger.LogError($"Exception occured during role get in service: {ex.Message}");
                throw;
            }
        }
        public async Task<OperationResult> ConfirmEmailAsync(EmailConfirmationViewModel emailConfirmationViewModel)
        {
            try
            {
                var userManager = identityService.UserManager;
                var user = await userManager.FindByEmailAsync(emailConfirmationViewModel.Email);
                if (user == null)
                {
                    return OperationResult.Failure($"user does not exist with the following Email: {emailConfirmationViewModel.Email}");
                }
                var result = await userManager.ConfirmEmailAsync(user, emailConfirmationViewModel.Token);
                if (result.Succeeded)
                {
                    return OperationResult.Success("Email has been Confirmed", user);
                }
                return OperationResult.Failure($"Invalid Token try again!");
            }
            catch (Exception ex)
            {
                logger.LogError($"Exception occured during email conformation in service: {ex.Message}");
                throw;
            }
        }
        public async Task<OperationResult> IsEmailAvailableForRegistrationAsync(string email)
        {
            try
            {
                var userManager = identityService.UserManager;
                var user = await userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    return OperationResult.Success($"Yes! this email available for registration: {email}", true);
                }
                else
                {
                    return OperationResult.Failure($"No! this email is not available for registration: {email}", false);
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Exception occured during emial check avaiablel or nor in service: {ex.Message}");
                throw;
            }
        }
        public async Task<OperationResult> ChangePasswordAsync(ChangePasswordViewModel changePasswordViewModel)
        {
            try
            {
                var userManager = identityService.UserManager;
                var user = await userManager.FindByEmailAsync(changePasswordViewModel.Email);
                if (user == null)
                {
                    return OperationResult.Failure($"user does not exist with the following Email: {changePasswordViewModel.Email}");
                }
                if (await userManager.CheckPasswordAsync(user, changePasswordViewModel.CurrentPassword))
                {
                    var result = await userManager.ChangePasswordAsync(user, changePasswordViewModel.CurrentPassword, changePasswordViewModel.NewPassword);
                    if (result.Succeeded)
                    {
                        return OperationResult.Success($"Password update succfully");
                    }
                    return OperationResult.Failure($"Password was not update:", result.Errors);
                }
                return OperationResult.Failure($"Current password was wrong:");

            }
            catch (Exception ex)
            {
                logger.LogError($"Exception occured during change password in service: {ex.Message}");
                throw;
            }
        }
        private async Task<Roles> GetUserRoleId(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user is null)
            {
                return null;
            }
            
            var userRole = await userManager.GetRolesAsync(user);
            if (userRole == null)
            {
                return null;
            }

            var firstRole = userRole.FirstOrDefault();

            if (string.IsNullOrWhiteSpace(firstRole))
            {
                return null;
            }

            var role = await roleManager.FindByNameAsync(firstRole);
            if(role != null)
            {

                return role;
            }

            return null;
        }
        public async Task<List<Claim>> GetUserPermissionClaimsAsync(string userId)
        {
            var role = await GetUserRoleId(userId);
            if (role is null)
            {
                return new List<Claim>();
            } 
            role = await roleManager.FindByIdAsync(role.Id.ToString());
            var claims = await roleManager.GetClaimsAsync(role);
            return claims.Where(x => x.Type == ClaimsConstants.PermissionClaim).ToList();
        }

    }
}

