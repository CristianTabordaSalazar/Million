using MongoDB.Driver;
using MillionApi.Domain.Entities;

namespace MillionApi.Infrastructure.Persistence
{
    public sealed class IndexesInitializer
    {
        private readonly MongoDbContext _ctx;

        public IndexesInitializer(MongoDbContext ctx) => _ctx = ctx;

        public async Task CreateAsync(CancellationToken ct = default)
        {
            // Index único por Name (case-insensitive)
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

            // Otros índices recomendados (búsquedas frecuentes)
            // Ej: Address
            var addressIndex = new CreateIndexModel<Property>(
                Builders<Property>.IndexKeys.Ascending(x => x.Address),
                new CreateIndexOptions { Name = "ix_properties_address" });
            await _ctx.Properties.Indexes.CreateOneAsync(addressIndex, cancellationToken: ct);
        }
    }
}
