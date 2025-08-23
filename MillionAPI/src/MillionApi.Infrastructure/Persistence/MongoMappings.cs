using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson;
using MillionApi.Domain.Entities;

namespace MillionApi.Infrastructure.Persistence
{
    public static class MongoMappings
    {
        private static bool _registered;

        public static void Register()
        {
            if (_registered) return;
            _registered = true;

            BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));

            var pack = new ConventionPack
            {
                new CamelCaseElementNameConvention(),
                new IgnoreExtraElementsConvention(true),
                new IgnoreIfNullConvention(true)
            };
            ConventionRegistry.Register("MillionApi Conventions", pack, _ => true);

            if (!BsonClassMap.IsClassMapRegistered(typeof(Property)))
            {
                BsonClassMap.RegisterClassMap<Property>(cm =>
                {
                    cm.AutoMap();

                    cm.MapIdProperty(p => p.Id)
                      .SetIdGenerator(GuidGenerator.Instance);
                });
            }
        }
    }
}
