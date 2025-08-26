using MongoDB.Driver;
using MillionApi.Domain.Entities;
using MillionApi.Infrastructure.Persistence;

namespace MillionApi.Infrastructure.Indexes
{
    public sealed class OwnerIndexesInitializer : IIndexesInitializer
    {
        private readonly MongoDbContext _ctx;

        public OwnerIndexesInitializer(MongoDbContext ctx) => _ctx = ctx;

        public async Task CreateAsync(CancellationToken ct = default)
        {
            // 1) Índice único por Name (case-insensitive)
            var nameKeys = Builders<Owner>.IndexKeys.Ascending(x => x.Name);

            var collation = new Collation(locale: "en", strength: CollationStrength.Secondary);
            var nameOpts = new CreateIndexOptions
            {
                Name = "ux_owners_name_ci",
                Collation = collation
            };

            await _ctx.Owners.Indexes.CreateOneAsync(
                new CreateIndexModel<Owner>(nameKeys, nameOpts),
                cancellationToken: ct
            );

            var addressIndex = new CreateIndexModel<Owner>(
                Builders<Owner>.IndexKeys.Ascending(x => x.Address),
                new CreateIndexOptions { Name = "ix_owners_address" }
            );

            await _ctx.Owners.Indexes.CreateOneAsync(addressIndex, cancellationToken: ct);

            var dobIndex = new CreateIndexModel<Owner>(
                Builders<Owner>.IndexKeys.Ascending(x => x.DateOfBirth),
                new CreateIndexOptions { Name = "ix_owners_dateOfBirth" }
            );

            await _ctx.Owners.Indexes.CreateOneAsync(dobIndex, cancellationToken: ct);

            var compoundKeys = Builders<Owner>.IndexKeys
                .Ascending(x => x.Name)
                .Ascending(x => x.DateOfBirth);

            var compoundOpts = new CreateIndexOptions<Owner>
            {
                Name = "ix_owners_name_dateOfBirth",
                Collation = collation
            };

            var compoundModel = new CreateIndexModel<Owner>(compoundKeys, compoundOpts);
            await _ctx.Owners.Indexes.CreateOneAsync(compoundModel, cancellationToken: ct);
        }
    }
}
