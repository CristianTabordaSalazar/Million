namespace MillionApi.Application.Services.Property
{
    public interface IPropertyService
    {
        /// <summary>
        /// Searches properties by optional filters and returns a page of results.
        /// </summary>
        /// <param name="name">Contains filter (case-insensitive).</param>
        /// <param name="address">Contains filter (case-insensitive).</param>
        /// <param name="minPrice">Minimum price (inclusive).</param>
        /// <param name="maxPrice">Maximum price (inclusive).</param>
        /// <param name="page">1-based page. Default 1.</param>
        /// <param name="pageSize">Items per page. Default 10, max 100.</param>
        /// <exception cref="ArgumentOutOfRangeException">If page/pageSize are out of bounds.</exception>
        Task<(IReadOnlyList<PropertyResult> Items, long Total)> SearchAsync(
            string? name, string? address, decimal? minPrice, decimal? maxPrice,
            int page, int pageSize, CancellationToken ct = default);

        /// <summary>Gets a property by id.</summary>
        /// <exception cref="NotFoundException">If not found.</exception>
        Task<PropertyResult> GetByIdAsync(Guid id, CancellationToken ct = default);

        /// <summary>Gets a property by its name (case-insensitive contains).</summary>
        /// <exception cref="NotFoundException">If not found.</exception>
        Task<PropertyResult?> GetPropertyByNameAsync(string name, CancellationToken ct = default);

        /// <summary>Gets a fully property (owner, images, traces).</summary>
        /// <exception cref="NotFoundException">If not found.</exception>
        Task<PropertyDetailResult> GetDetailByIdAsync(Guid id, CancellationToken ct = default);
    }
}