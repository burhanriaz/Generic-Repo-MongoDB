using Cohere.Domain.Models.Account;
using Web.Domain.Infrastructure;
using Web.Domain.Models.Account;

namespace Web.Domain.Service.Abstractions
{
    public interface IAuthService
    {
        Task<OperationResult> SignInAsync(LoginViewModel loginVm, bool lockoutOnFailure);
    }

}
