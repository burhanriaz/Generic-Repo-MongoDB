using Web.Entity.Entity.Identity;
using Web.Domain.Models.Account;
using Web.Domain.Service.Abstractions.Generic;
using Web.Domain.Infrastructure;
using System.Security.Claims;

namespace Web.Domain.Service.Abstractions
{
    public interface IAccountService<TViewModel, TEntity> : IServiceAsync<TViewModel, TEntity>
     where TViewModel : UserViewModel
     where TEntity : Users
    {
        Task<OperationResult> CreateUserAsync(UserViewModel userVm);
        Task<OperationResult> UpdateUserAsync(UserViewModel userVm);
        Task<OperationResult> GetAllUsersAsync();
        Task<OperationResult> GetUserByIdAsync(Guid userId);
        Task<OperationResult> GetUserByEmail(string email);
        Task<OperationResult> DeleteUserByIdAsync(Guid userId);
        Task<OperationResult> CreateRoleAsync(RolesViewModel roleVm);
        Task<OperationResult> UpdateRoleAsync(RolesViewModel roleVm);
        Task<OperationResult> GetAllRolesAsync();
        Task<OperationResult> GetRoleByIdAsync(Guid userId);
        Task<OperationResult> DeleteRoleByIdAsync(Guid userId);
        Task<OperationResult> ConfirmEmailAsync(EmailConfirmationViewModel emailConfirmationViewModel);
        Task<OperationResult> IsEmailAvailableForRegistrationAsync(string email);
        Task<OperationResult> ChangePasswordAsync(ChangePasswordViewModel changePasswordViewModel);
        Task<List<Claim>> GetUserPermissionClaimsAsync(string UserId);

    }

    //public interface IRoleSwitchingService
    //{
    //    Task<OperationResult> SwitchFromClientToCoach(string accountId, SwitchFromClientToCoachViewModel model);

    //    Task<OperationResult<string>> SwitchFromCoachToClient(string accountId);
    //}
}
