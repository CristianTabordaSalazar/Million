using MillionApi.Application.Common.Interfaces.Persistence;
using MillionApi.Application.Services.Property;

namespace MillionApi.Application.Service.Property
{
    public class PropertyService : IPropertyService
    {
        private readonly IPropertyRepository _propertyRepository;

        public PropertyService(IPropertyRepository propertyRepository)
        {
            _propertyRepository = propertyRepository;
        }

        public async Task<PropertyResult?> GetPropertyByNameAsync(string name)
        {
            var property = await _propertyRepository.GetByNameAsync(name);
            if (property == null) return null;

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