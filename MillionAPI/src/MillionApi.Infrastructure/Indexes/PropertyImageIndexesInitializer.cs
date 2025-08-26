using MongoDB.Driver;
using MongoDB.Bson;
using MillionApi.Domain.Entities;
using MillionApi.Infrastructure.Persistence;

namespace MillionApi.Infrastructure.Indexes
{
    public sealed class PropertyImageIndexesInitializer : IIndexesInitializer
    {
        private readonly MongoDbContext _ctx;

        public PropertyImageIndexesInitializer(MongoDbContext ctx) => _ctx = ctx;

        public async Task CreateAsync(CancellationToken ct = default)
        {
            // 1. Index on PropertyId
            var propertyIdIndex = new CreateIndexModel<PropertyImage>(
                Builders<PropertyImage>.IndexKeys.Ascending(x => x.PropertyId),
                new CreateIndexOptions { Name = "ix_propertyImages_propertyId" }
            );
            await _ctx.PropertyImages.Indexes.CreateOneAsync(propertyIdIndex, cancellationToken: ct);

            // 2. Compound unique index on (PropertyId, Url)
            var compoundKeys = Builders<PropertyImage>.IndexKeys
                .Ascending(x => x.PropertyId)
                .Ascending(x => x.Url);

            var compoundOpts = new CreateIndexOptions<PropertyImage>
            {
                Name = "ux_propertyImages_propertyId_url",
                Unique = true,
                PartialFilterExpression = new BsonDocument("Url", new BsonDocument("$type", "string"))
            };

            var compoundModel = new CreateIndexModel<PropertyImage>(compoundKeys, compoundOpts);
            await _ctx.PropertyImages.Indexes.CreateOneAsync(compoundModel, cancellationToken: ct);
        }
    }
}
