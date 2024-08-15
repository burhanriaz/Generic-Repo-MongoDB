using AutoMapper;
using Web.Domain.Models.Account;
using Web.Entity.Entity.Identity;
using Web.Entity.UnitOfWork;

namespace Web.Domain.Mapping.CustomResolvers
{
    public class RolesResolver : IValueResolver<Users, UserViewModel, List<RolesViewModel>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RolesResolver(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public List<RolesViewModel> Resolve(Users source, UserViewModel destination, List<RolesViewModel> destMember, ResolutionContext context)
        {
            var roleIds = source.Roles; // Assuming source.Roles is a list of role IDs
            var roles = _unitOfWork.GetGenericRepositoryAsync<Roles>().Get(x => roleIds.Contains(x.Id)).Result.ToList(); // Implement GetRolesByIds method
            return _mapper.Map<List<RolesViewModel>>(roles);
        }
    }
}
