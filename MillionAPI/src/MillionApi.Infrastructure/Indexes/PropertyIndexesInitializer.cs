using MongoDB.Driver;
using MillionApi.Domain.Entities;
using MillionApi.Infrastructure.Persistence;
using MongoDB.Bson;

namespace MillionApi.Infrastructure.Indexes
{
    public sealed class PropertyIndexesInitializer : IIndexesInitializer
    {
        private readonly MongoDbContext _ctx;

        public PropertyIndexesInitializer(MongoDbContext ctx) => _ctx = ctx;

        public async Task CreateAsync(CancellationToken ct = default)
        {
            // 1. Unique case-insensitive index on Name
            var nameKeys = Builders<Property>.IndexKeys.Ascending(x => x.Name);
            var collation = new Collation(locale: "en", strength: CollationStrength.Secondary);
            var nameOpts = new CreateIndexOptions<Property>
            {
                Name = "ux_properties_name_ci",
                Collation = collation,
                PartialFilterExpression = new BsonDocument("name", new BsonDocument("$type", "string"))
            };

            try { await _ctx.Properties.Indexes.DropOneAsync("ux_properties_name_ci", ct); } catch { /* ignore */ }
            var nameModel = new CreateIndexModel<Property>(nameKeys, nameOpts);
            await _ctx.Properties.Indexes.CreateOneAsync(nameModel, cancellationToken: ct);

            // 2. Index on Address
            var addressIndex = new CreateIndexModel<Property>(
                Builders<Property>.IndexKeys.Ascending(x => x.Address),
                new CreateIndexOptions { Name = "ix_properties_address" }
            );
            await _ctx.Properties.Indexes.CreateOneAsync(addressIndex, cancellationToken: ct);

            // 3. Index on Price
            var priceIndex = new CreateIndexModel<Property>(
                Builders<Property>.IndexKeys.Ascending(x => x.Price),
                new CreateIndexOptions { Name = "ix_properties_price" }
            );
            await _ctx.Properties.Indexes.CreateOneAsync(priceIndex, cancellationToken: ct);

            // 4. Unique index on CodeInternal
            var codeInternalIndex = new CreateIndexModel<Property>(
                Builders<Property>.IndexKeys.Ascending(x => x.CodeInternal),
                new CreateIndexOptions { Name = "ux_properties_codeInternal", Unique = true }
            );
            await _ctx.Properties.Indexes.CreateOneAsync(codeInternalIndex, cancellationToken: ct);

            // 5. Index on OwnerId
            var ownerIdIndex = new CreateIndexModel<Property>(
                Builders<Property>.IndexKeys.Ascending(x => x.OwnerId),
                new CreateIndexOptions { Name = "ix_properties_ownerId" }
            );
            await _ctx.Properties.Indexes.CreateOneAsync(ownerIdIndex, cancellationToken: ct);

            // 6. Compound index on (Price, Year)
            var compoundKeys = Builders<Property>.IndexKeys
                .Ascending(x => x.Price)
                .Ascending(x => x.Year);

            var compoundOpts = new CreateIndexOptions<Property>
            {
                Name = "ix_properties_price_year"
            };

            var compoundModel = new CreateIndexModel<Property>(compoundKeys, compoundOpts);
            await _ctx.Properties.Indexes.CreateOneAsync(compoundModel, cancellationToken: ct);
        }
    }
}
