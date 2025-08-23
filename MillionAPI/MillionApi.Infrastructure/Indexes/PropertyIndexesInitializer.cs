using MongoDB.Driver;
using MillionApi.Domain.Entities;
using MillionApi.Infrastructure.Persistence;

namespace MillionApi.Infrastructure.Indexes
{
    public sealed class PropertyIndexesInitializer : IIndexesInitializer
    {
        private readonly MongoDbContext _ctx;

        public PropertyIndexesInitializer(MongoDbContext ctx) => _ctx = ctx;

        public async Task CreateAsync(CancellationToken ct = default)
        {
            var keys = Builders<Property>.IndexKeys.Ascending(x => x.Name);

            var collation = new Collation(locale: "en", strength: CollationStrength.Secondary);
            var opts = new CreateIndexOptions
            {
                Name = "ux_properties_name_ci",
                Unique = true,
                Collation = collation
            };

            var model = new CreateIndexModel<Property>(keys, opts);
            await _ctx.Properties.Indexes.CreateOneAsync(model, cancellationToken: ct);

            var addressIndex = new CreateIndexModel<Property>(
                Builders<Property>.IndexKeys.Ascending(x => x.Address),
                new CreateIndexOptions { Name = "ix_properties_address" }
            );

            await _ctx.Properties.Indexes.CreateOneAsync(addressIndex, cancellationToken: ct);
        }
    }
}
