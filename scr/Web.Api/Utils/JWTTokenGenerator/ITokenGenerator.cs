using Web.Domain.Models.Account;
using Web.Entity.Entity.Identity;
namespace Web.Api.Utils.JWTTokenGenerator
{
    public interface ITokenGenerator
    {
        string GenerateToken(UserViewModel user);
    }
}

