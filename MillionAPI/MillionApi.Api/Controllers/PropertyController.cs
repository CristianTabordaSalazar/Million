using Microsoft.AspNetCore.Mvc;
using MillionApi.Application.Services.Property;
using MillionApi.Contracts.Property;

namespace MillionApi.Api.Controllers
{
    [ApiController]
    [Route("api")]
    public class PropertyController : ControllerBase
    {
        private readonly IPropertyService _propertyService;
        public PropertyController(IPropertyService propertyService)
        {
            _propertyService = propertyService;
        }

        [HttpGet("property/{name}")]
        [ProducesResponseType(typeof(PropertyResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByName([FromRoute] GetPropertyByNameRequest req, CancellationToken ct)
        {
            var propertyResult = await _propertyService.GetPropertyByNameAsync(req.Name.Trim(), ct);

            var response = new PropertyResponse(
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