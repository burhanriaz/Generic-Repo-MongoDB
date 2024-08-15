using AutoMapper;
using Web.Entity.Entity.Identity;
using Web.Domain.Models.Account;
using Web.Domain.Mapping.CustomResolvers;
namespace Web.Domain.Mapping
{
    public class MappingProfile : Profile 
    {
        public MappingProfile()
        {
            #region User
            // Mapping from UserViewModel to Users
            CreateMap<UserViewModel, Users>()
                .ForMember(dest => dest.NormalizedUserName, opt => opt.MapFrom(src => src.Email.ToUpper()))
                .ForMember(dest => dest.NormalizedEmail, opt => opt.MapFrom(src => src.Email.ToUpper()))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.Roles.Select(x => x.Id)));
             
            // Mapping from Users to UserViewModel
            CreateMap<Users, UserViewModel>()
                .ForMember(dest => dest.Roles, opt => opt.MapFrom<RolesResolver>())
                .BeforeMap((src, dest) =>
                {
                    // Ignore properties not in UserViewModel here
                    dest.Password = null;
                    dest.ConfirmPassword = null;
                    // Add other properties to ignore as needed
                });
            #endregion

            #region Roles 
            // Mapping from RolesViewModel to Roles
            CreateMap<RolesViewModel, Roles>()
                .ForMember(dest => dest.Claims, opt => opt.MapFrom(src => src.Permissions.ToList()));

            // Mapping from Roles to RolesViewModel
            CreateMap<Roles, RolesViewModel>()
                .ForMember(dest => dest.Permissions, opt => opt.MapFrom(src => src.Claims.Select(x=>x.Value)));


            #endregion
        }
    }
}