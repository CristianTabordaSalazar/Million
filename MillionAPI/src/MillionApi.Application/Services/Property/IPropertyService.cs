namespace MillionApi.Application.Services.Property
{
    public interface IPropertyService
    {
        Task<(IReadOnlyList<PropertyResult> Items, long Total)> SearchAsync(
            string? name, string? address, decimal? minPrice, decimal? maxPrice,
            int page, int pageSize, CancellationToken ct = default);

        Task<PropertyResult> GetByIdAsync(Guid id, CancellationToken ct = default);

        Task<PropertyResult?> GetPropertyByNameAsync(string name, CancellationToken ct = default);

        Task<PropertyDetailResult> GetDetailByIdAsync(Guid id, CancellationToken ct = default);
    }
}