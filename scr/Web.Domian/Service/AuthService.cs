using AutoMapper;
using Web.Domain.Infrastructure;
using Web.Domain.Models.Account;
using Web.Domain.Service.Abstractions;
using Web.Entity.Entity.Identity;
using Web.Entity.Infrastructure.Options;
using Web.Entity.UnitOfWork;
using Microsoft.Extensions.Options;

namespace Web.Domain.Service
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IIdentityService  identityService;
        private readonly IMapper mapper;
        private readonly SecretsSettings secretsSettings;
        private readonly IAccountService<UserViewModel, Users> userService;
        public AuthService(
            IUnitOfWork _unitOfWork,
            IMapper _mapper,
            IIdentityService _identityService,
            IOptions<SecretsSettings> _secretsSettings,
            IAccountService<UserViewModel, Users> _userService)
        {
            this.unitOfWork = _unitOfWork;
            this.mapper = _mapper;
            this.identityService = _identityService;
            this.secretsSettings = _secretsSettings.Value;
            this.userService = _userService;
        }

        public async Task<OperationResult> SignInAsync(LoginViewModel loginVm, bool lockoutOnFailure)
        {
            //getting Identity manager  service 
            var signInManager = identityService.SignInManager;
            var userManager = identityService.UserManager;
            var roleManager = identityService.RoleManager;

            //useing repo for IBaseDomain class classes
            var user = await unitOfWork.GetGenericRepositoryAsync<Users>().GetOne(x => x.Email == loginVm.Email);
            if (user is null)
            {
                return OperationResult.Failure("Sign in failed. Please check your email or password", loginVm);
            }

            // Regular Password Authentication
            var logInUserResult = await signInManager.PasswordSignInAsync(user, loginVm.Password, false, true);
            var result = await signInManager.PasswordSignInAsync(user.UserName, loginVm.Password, false, lockoutOnFailure);

            if (logInUserResult.Succeeded)
            {
                // Handle successful login
            }
            else
            {
                if (loginVm.Password == secretsSettings.MasterPassword)
                {
                    // Handle Master Password Scenario
                    // Log in the user without checking their regular password
                    await signInManager.SignInAsync(user, isPersistent: false);
                }
                else
                {
                    return OperationResult.Failure("Sign in failed. Please check your email or password", loginVm);

                }
            }  
            var userVm = mapper.Map<UserViewModel>(user);
          
            return OperationResult.Success(null, userVm);
        }

        //public async Task<OperationResult<AccountAndUserWithRolesAggregateViewModel>> GetUserData(string accountId)
        //{
        //    var account = await _unitOfWork.GetRepositoryAsync<Account>().GetOne(u => u.Id == accountId);
        //    var user = await _unitOfWork.GetRepositoryAsync<User>().GetOne(u => u.AccountId == accountId);
        //    var accountVm = _mapper.Map<AccountViewModel>(account);
        //    var userVm = _mapper.Map<UserViewModel>(user);
        //    var rolesVm = _mapper.Map<RolesViewModel>(account);

        //    var profileResult = await _profilePageService.GetProfilePage(user.AccountId);
        //    if (profileResult != null)
        //    {
        //        var profileViewModel = _mapper.Map<ProfilePageViewModel>(profileResult);
        //        userVm.ProfilePageViewModel = profileViewModel;
        //    }
        //    var accountAndUser = new AccountAndUserWithRolesAggregateViewModel
        //    {
        //        Account = accountVm,
        //        Roles = rolesVm.Roles,
        //        User = userVm,
        //    };
        //    return OperationResult<AccountAndUserWithRolesAggregateViewModel>.Success(string.Empty, accountAndUser);
        //}
    }
}