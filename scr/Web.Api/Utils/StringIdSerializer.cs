//using MongoDB.Bson;
//using MongoDB.Bson.Serialization;
//using MongoDB.Bson.Serialization.Serializers;
//namespace Web.Api.Utils
//{
//    public class StringIdSerializer : SerializerBase<string>
//    {
//        public override string Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
//        {
//            var bsonReader = context.Reader;
//            var bsonType = bsonReader.GetCurrentBsonType();
//            switch (bsonType)
//            {
//                case BsonType.String:
//                    return bsonReader.ReadString();
//                case BsonType.ObjectId:
//                    return bsonReader.ReadObjectId().ToString();
//                default:
//                    var message = $"Cannot deserialize StringId from BsonType: {bsonType}";
//                    throw new BsonSerializationException(message);
//            }
//        }

//        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, string value)
//        {
//            var bsonWriter = context.Writer;
//            bsonWriter.WriteString(value);
//        }
//    }

//}
