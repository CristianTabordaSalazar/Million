using MillionApi.Domain.Entities;

namespace MillionApi.Application.Common.Interfaces.Persistence
{
    public interface IPropertyRepository
    {


        Task<(IReadOnlyList<Property> Items, long Total)> SearchAsync(
            string? name, string? address, decimal? minPrice, decimal? maxPrice,
            int page, int pageSize, CancellationToken ct = default);
        Task<Property?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<Property?> GetByNameAsync(string name, CancellationToken ct = default);
        Task<(Property Property, Owner? Owner, PropertyImage? FirstImage, List<PropertyTrace> Traces)?> GetDetailByIdAsync(
            Guid id, CancellationToken ct = default);
    }
}
