using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;

namespace Web.Entity.Infrastructure
{
    internal class MongoIgnoreDefaultValuesConvention : IMemberMapConvention
    {
        public string Name => "Ignore default properties for all classes";

        public void Apply(BsonMemberMap memberMap)
        {
            memberMap.SetIgnoreIfDefault(true);

        }
    }
}
