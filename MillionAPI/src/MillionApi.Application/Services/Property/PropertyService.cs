using MillionApi.Application.Common.Exceptions;
using MillionApi.Application.Common.Interfaces.Persistence;
using MillionApi.Application.Services.Owner;

namespace MillionApi.Application.Services.Property
{
    public class PropertyService : IPropertyService
    {
        private readonly IPropertyRepository _propertyRepository;

        public PropertyService(IPropertyRepository propertyRepository)
        {
            _propertyRepository = propertyRepository;
        }

        public async Task<(IReadOnlyList<PropertyResult> Items, long Total)> SearchAsync(
            string? name, string? address, decimal? minPrice, decimal? maxPrice,
            int page, int pageSize, CancellationToken ct = default)
        {
            var (properties, total) = await _propertyRepository.SearchAsync(name, address, minPrice, maxPrice, page, pageSize, ct);
            var items = properties.Select(e => new PropertyResult(e.Id, e.Name, e.Address, e.Price, e.CodeInternal, e.Year)).ToList();
            return (items, total);
        }

        public async Task<PropertyResult> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            var property = await _propertyRepository.GetByIdAsync(id, ct);

            if (property is null)
                throw new NotFoundException(nameof(Property), id.ToString());

            return new PropertyResult(
                property.Id,
                property.Name,
                property.Address,
                property.Price,
                property.CodeInternal,
                property.Year
            );
        }

        public async Task<PropertyResult?> GetPropertyByNameAsync(string name, CancellationToken ct = default)
        {
            var property = await _propertyRepository.GetByNameAsync(name, ct);

            if (property is null)
                throw new NotFoundException(nameof(Property), name);

            return new PropertyResult(
                property.Id,
                property.Name,
                property.Address,
                property.Price,
                property.CodeInternal,
                property.Year
            );
        }

        public async Task<PropertyDetailResult> GetDetailByIdAsync(Guid id, CancellationToken ct = default)
        {
            var detail = await _propertyRepository.GetDetailByIdAsync(id, ct);

            if (detail is null)
                throw new NotFoundException(nameof(Property), id.ToString());

            var (property, owner, firstImage, traces) = detail.Value;

            return new PropertyDetailResult(
                property.Id,
                property.Name,
                property.Address,
                property.Price,
                property.CodeInternal,
                property.Year,
                owner is null ? null : new OwnerResult(owner.Id, owner.Name, owner.Address, owner.Photo, owner.DateOfBirth),
                firstImage?.Url,
                traces.Select(t => new PropertyTraceResult(t.Id, t.DateSale, t.Name, t.Value, t.Tax)).ToList()
            );
        }
    }
}