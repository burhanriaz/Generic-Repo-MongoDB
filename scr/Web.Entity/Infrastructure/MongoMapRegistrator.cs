using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;
using Web.Entity;
using Web.Entity.Infrastructure;

namespace Web.Entity.Infrastructure
{
    public static class MongoMapRegistrator
    {
        // Use mapping with code (not attributes) is more generic way if MongoDB will be one day replaces by other DB.
        public static void RegisterMaps()
        {
            var pack = new ConventionPack
            {
                new MongoIgnoreDefaultValuesConvention(),
                new EnumRepresentationConvention(BsonType.String),
                new IgnoreExtraElementsConvention(true),
            };
            ConventionRegistry.Register("Custom ignore default", pack, t => true);

             BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));

            BsonClassMap.RegisterClassMap<BaseEntity>(cm =>
            {
                cm.AutoMap();
                cm.MapIdMember(c => c.Id).SetIdGenerator(GuidGenerator.Instance);
            });

        }


    }
}
