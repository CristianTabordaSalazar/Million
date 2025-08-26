using MongoDB.Driver;
using MongoDB.Bson;
using MillionApi.Domain.Entities;
using MillionApi.Infrastructure.Persistence;

namespace MillionApi.Infrastructure.Indexes
{
    public sealed class PropertyTraceIndexesInitializer : IIndexesInitializer
    {
        private readonly MongoDbContext _ctx;

        public PropertyTraceIndexesInitializer(MongoDbContext ctx) => _ctx = ctx;

        public async Task CreateAsync(CancellationToken ct = default)
        {
            // 1. Index on PropertyId
            var propertyIdIndex = new CreateIndexModel<PropertyTrace>(
                Builders<PropertyTrace>.IndexKeys.Ascending(x => x.PropertyId),
                new CreateIndexOptions { Name = "ix_propertyTraces_propertyId" }
            );
            await _ctx.PropertyTraces.Indexes.CreateOneAsync(propertyIdIndex, cancellationToken: ct);

            // 2. Compound unique index on (PropertyId, DateSale, Name)
            var compoundKeys = Builders<PropertyTrace>.IndexKeys
                .Ascending(x => x.PropertyId)
                .Ascending(x => x.DateSale)
                .Ascending(x => x.Name);

            var compoundOpts = new CreateIndexOptions<PropertyTrace>
            {
                Name = "ux_propertyTraces_propertyId_dateSale_name",
                Unique = true,
                PartialFilterExpression = new BsonDocument("Name", new BsonDocument("$type", "string"))
            };

            var compoundModel = new CreateIndexModel<PropertyTrace>(compoundKeys, compoundOpts);
            await _ctx.PropertyTraces.Indexes.CreateOneAsync(compoundModel, cancellationToken: ct);
        }
    }
}
