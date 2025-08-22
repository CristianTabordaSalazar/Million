using MongoDB.Driver;
using MillionApi.Domain.Entities;
using MillionApi.Application.Services.Property;
using MillionApi.Infrastructure.Persistence;
using MillionApi.Application.Common.Interfaces.Persistence;

namespace MillionApi.Infrastructure.Repositories
{
    public sealed class PropertyRepository : IPropertyRepository
    {
        private readonly MongoDbContext _ctx;

        public PropertyRepository(MongoDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<Property?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            var filter = Builders<Property>.Filter.Eq(p => p.Id, id);
            return await _ctx.Properties.Find(filter).FirstOrDefaultAsync(ct);
        }

        public async Task<Property?> GetByNameAsync(string name, CancellationToken ct = default)
        {
            // Usa collation case-insensitive para coincidir con el índice
            var filter = Builders<Property>.Filter.Eq(p => p.Name, name);
            var options = new FindOptions<Property> { Collation = new Collation("en", strength: CollationStrength.Secondary) };
            using var cursor = await _ctx.Properties.FindAsync(filter, options, ct);
            return await cursor.FirstOrDefaultAsync(ct);
        }

        public async Task<bool> ExistsByNameAsync(string name, CancellationToken ct = default)
        {
            var filter = Builders<Property>.Filter.Eq(p => p.Name, name);
            var options = new CountOptions { Collation = new Collation("en", strength: CollationStrength.Secondary) };
            var count = await _ctx.Properties.CountDocumentsAsync(filter, options, ct);
            return count > 0;
        }

        public async Task<IReadOnlyList<Property>> SearchAsync(
            string? name = null,
            int page = 1,
            int pageSize = 20,
            CancellationToken ct = default)
        {
            var filter = string.IsNullOrWhiteSpace(name)
                ? Builders<Property>.Filter.Empty
                : Builders<Property>.Filter.Regex(p => p.Name, new MongoDB.Bson.BsonRegularExpression(name, "i"));

            var opts = new FindOptions<Property>
            {
                Skip = (page - 1) * pageSize,
                Limit = pageSize
            };

            var cursor = await _ctx.Properties.FindAsync(filter, opts, ct);
            return await cursor.ToListAsync(ct);
        }

        public async Task AddAsync(Property entity, CancellationToken ct = default)
        {
            await _ctx.Properties.InsertOneAsync(entity, cancellationToken: ct);
        }

        public async Task UpdateAsync(Property entity, CancellationToken ct = default)
        {
            var filter = Builders<Property>.Filter.Eq(p => p.Id, entity.Id);
            var result = await _ctx.Properties.ReplaceOneAsync(filter, entity, cancellationToken: ct);
            if (result.MatchedCount == 0)
                throw new KeyNotFoundException($"Property {entity.Id} not found.");
        }

        public async Task DeleteAsync(Guid id, CancellationToken ct = default)
        {
            var filter = Builders<Property>.Filter.Eq(p => p.Id, id);
            await _ctx.Properties.DeleteOneAsync(filter, ct);
        }
    }
}
