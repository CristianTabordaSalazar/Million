using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MillionApi.Domain.Entities;
using MillionApi.Infrastructure.Options;

namespace MillionApi.Infrastructure.Persistence
{
    public sealed class MongoDbContext
    {
        private readonly IMongoDatabase _db;
        private readonly MongoSettings _opts;

        public MongoDbContext(IMongoClient client, IOptions<MongoSettings> options)
        {
            _opts = options.Value;
            _db = client.GetDatabase(_opts.Database);
        }

        public IMongoCollection<Property> Properties
            => _db.GetCollection<Property>(_opts.PropertyCollectionName);

        public IMongoCollection<Owner> Owners
            => _db.GetCollection<Owner>(_opts.OwnerCollectionName);
    }
}
