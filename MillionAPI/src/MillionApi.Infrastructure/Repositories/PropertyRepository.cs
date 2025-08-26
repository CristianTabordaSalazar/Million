using MongoDB.Driver;
using MillionApi.Domain.Entities;
using MillionApi.Application.Services.Property;
using MillionApi.Infrastructure.Persistence;
using MillionApi.Application.Common.Interfaces.Persistence;
using MongoDB.Bson;

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
            var filter = Builders<Property>.Filter.Eq(p => p.Name, name);
            var options = new FindOptions<Property> { Collation = new Collation("en", strength: CollationStrength.Secondary) };
            using var cursor = await _ctx.Properties.FindAsync(filter, options, ct);
            return await cursor.FirstOrDefaultAsync(ct);
        }

        public async Task<(IReadOnlyList<Property> Items, long Total)> SearchAsync(
            string? name, string? address, decimal? minPrice, decimal? maxPrice,
            int page, int pageSize, CancellationToken ct = default)
        {
            var fb = Builders<Property>.Filter;
            var filter = fb.Empty;

            if (!string.IsNullOrWhiteSpace(name))
            {
                var regex = new BsonRegularExpression(name, "i");
                filter &= fb.Regex(p => p.Name, regex);
            }

            if (!string.IsNullOrWhiteSpace(address))
            {
                var regex = new BsonRegularExpression(address, "i");
                filter &= fb.Regex(p => p.Address, regex);
            }

            if (minPrice.HasValue)
                filter &= fb.Gte(p => p.Price, minPrice.Value);

            if (maxPrice.HasValue)
                filter &= fb.Lte(p => p.Price, maxPrice.Value);

            var find = _ctx.Properties.Find(filter);

            find = find.SortBy(p => p.Name);

            var total = await find.CountDocumentsAsync(ct);

            var skip = (page - 1) * pageSize;
            var items = await find.Skip(skip).Limit(pageSize).ToListAsync(ct);

            return (items, total);
        }

        public async Task<(Property Property, Owner? Owner, PropertyImage? FirstImage, List<PropertyTrace> Traces)?>GetDetailByIdAsync(
            Guid id, CancellationToken ct = default)
        {
            var filter = Builders<Property>.Filter.Eq(p => p.Id, id);
            var property = await _ctx.Properties.Find(filter).FirstOrDefaultAsync(ct);

            if (property is null)
                return null;

            // Owner
            Owner? owner = null;
            if (property.OwnerId != Guid.Empty)
            {
                var ownerFilter = Builders<Owner>.Filter.Eq(o => o.Id, property.OwnerId);
                owner = await _ctx.Owners.Find(ownerFilter).FirstOrDefaultAsync(ct);
            }

            // First enabled image
            var imgFilter = Builders<PropertyImage>.Filter.Eq(i => i.PropertyId, property.Id);
            var firstImage = await _ctx.PropertyImages.Find(imgFilter).FirstOrDefaultAsync(ct);

            // Traces
            var traceFilter = Builders<PropertyTrace>.Filter.Eq(t => t.PropertyId, property.Id);
            var traces = await _ctx.PropertyTraces.Find(traceFilter).ToListAsync(ct);

            return (property, owner, firstImage, traces);
        }
    }
}
