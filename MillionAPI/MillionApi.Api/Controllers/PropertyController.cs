using Microsoft.AspNetCore.Mvc;
using MillionApi.Application.Services.Property;
using MillionApi.Contracts.Property;

namespace MillionApi.Api.Controllers
{
    [ApiController]
    [Route("api/property")]
    public class PropertyController : ControllerBase
    {
        private readonly IPropertyService _propertyService;
        public PropertyController(IPropertyService propertyService)
        {
            _propertyService = propertyService;
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            var propertyResult = await _propertyService.GetPropertyByNameAsync(name);
            if (propertyResult is null)
            {
                return NotFound($"Property with name '{name}' not found.");
            }

            var response =  new PropertyResponse(
                propertyResult.Id,
                propertyResult.Name,
                propertyResult.Address,
                propertyResult.Price,
                propertyResult.CodeInternal,
                propertyResult.Year
            );
            return Ok(response);
        }
    }
}