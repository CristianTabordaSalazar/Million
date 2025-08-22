using MillionApi.Domain.Entities;

namespace MillionApi.Application.Common.Interfaces.Persistence
{
    public interface IPropertyRepository
    {
        // Task<Property?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<Property?> GetByNameAsync(string name, CancellationToken ct = default);
        // Task<bool> ExistsByNameAsync(string name, CancellationToken ct = default);

        // Task<IReadOnlyList<Property>> SearchAsync(
        //     string? name = null,
        //     int page = 1,
        //     int pageSize = 20,
        //     CancellationToken ct = default);

        // Task AddAsync(Property entity, CancellationToken ct = default);
        // Task UpdateAsync(Property entity, CancellationToken ct = default);
        // Task DeleteAsync(Guid id, CancellationToken ct = default);
    }
}
