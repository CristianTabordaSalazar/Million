using Microsoft.AspNetCore.Mvc;
using MillionApi.Application.Services.Property;
using MillionApi.Contracts.Property;
using MillionApi.Contracts.Common;
using MillionApi.Contracts.Owner;

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

        [HttpGet("properties")]
        [ProducesResponseType(typeof(PagedResponse<PropertyResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> List([FromQuery] PropertiesQuery q, CancellationToken ct)
        {
            var (items, total) = await _propertyService.SearchAsync(
                name: q.Name?.Trim(),
                address: q.Address?.Trim(),
                minPrice: q.MinPrice,
                maxPrice: q.MaxPrice,
                page: q.Page <= 0 ? 1 : q.Page,
                pageSize: q.PageSize is <= 0 or > 100 ? 10 : q.PageSize,
                ct: ct
            );

            var response = new PagedResponse<PropertyResponse>(
                items.Select(p => new PropertyResponse(
                    p.Id, p.Name, p.Address, p.Price, p.CodeInternal, p.Year
                )).ToList(),
                total
            );

            return Ok(response);
        }

        [HttpGet("properties/{id:guid}")]
        [ProducesResponseType(typeof(PropertyResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken ct)
        {
            var propertyResult = await _propertyService.GetByIdAsync(id, ct);

            var response = new PropertyResponse(
                propertyResult.Id,
                propertyResult.Name,
                propertyResult.Address,
                propertyResult.Price,
                propertyResult.CodeInternal,
                propertyResult.Year);

            return Ok(response);
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

        [HttpGet("properties/{id:guid}/detail")]
        [ProducesResponseType(typeof(PropertyDetailResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDetailById([FromRoute] Guid id, CancellationToken ct)
        {
            var result = await _propertyService.GetDetailByIdAsync(id, ct);

            var response = new PropertyDetailResponse(
                result.Id,
                result.Name,
                result.Address,
                result.Price,
                result.CodeInternal,
                result.Year,
                result.Owner is null
                    ? new OwnerResponse(Guid.Empty, "", "", "", DateTime.MinValue)
                    : new OwnerResponse(result.Owner.Id, result.Owner.Name, result.Owner.Address, result.Owner.Photo, result.Owner.DateOfBirth),
                result.FirstImageUrl,
                result.Traces.Select(t => new PropertyTraceResponse(t.Id, t.DateSale, t.Name, t.Value, t.Tax)).ToList()
            );

            return Ok(response);
        }
    }
}