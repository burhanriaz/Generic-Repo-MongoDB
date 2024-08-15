using AspNetCore.Identity.MongoDbCore.Models;
using Web.Entity;

namespace Web.Domain.Models.Account
{
    public class UserViewModel : IBaseDomain
    {
        public List<RolesViewModel> Roles { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }
        public bool EmailConfirmed { get; set; }
        public Guid Id { get; set; } 
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        ///Guid IBaseDomain.Id { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}