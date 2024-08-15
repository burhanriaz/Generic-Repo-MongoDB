using AspNetCore.Identity.MongoDbCore.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDbGenericRepository.Attributes;

namespace Web.Entity.Entity.Identity
{
    [CollectionName("Users")]
    //Attriube [CollectionName("Users")] specifies that this class will be mapped to “Users” collection in MongoDB.
    public class Users : MongoIdentityUser<Guid>, IBaseEntity
    {    
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
    }

}
