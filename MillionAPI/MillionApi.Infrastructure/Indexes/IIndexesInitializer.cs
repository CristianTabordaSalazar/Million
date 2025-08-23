
namespace MillionApi.Infrastructure.Indexes
{
    public interface IIndexesInitializer
    {
        Task CreateAsync(CancellationToken ct = default);
    }
}
