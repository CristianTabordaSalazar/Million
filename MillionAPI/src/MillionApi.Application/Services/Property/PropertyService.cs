using MillionApi.Application.Common.Exceptions;
using MillionApi.Application.Common.Interfaces.Persistence;

namespace MillionApi.Application.Services.Property
{
    public class PropertyService : IPropertyService
    {
        private readonly IPropertyRepository _propertyRepository;

        public PropertyService(IPropertyRepository propertyRepository)
        {
            _propertyRepository = propertyRepository;
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
    }
}