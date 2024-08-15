using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository.Attributes;

namespace Web.Entity.Entity.Identity
{
    [CollectionName("Roles")]
    public class Roles : MongoIdentityRole<Guid>, IBaseEntity
    {
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}
