using Microsoft.AspNetCore.Mvc;
using MillionApi.Application.Services.Property;
using MillionApi.Contracts.Property;
using MillionApi.Contracts.Common;
using MillionApi.Contracts.Owner;

namespace MillionApi.Api.Controllers
{
    /// <summary>
    /// Properties endpoints for listing, retrieving and getting properties.
    /// </summary>
    /// <remarks>
    /// Base route: <c>/api</c>.
    /// </remarks>
    [ApiController]
    [Route("api")]
    public class PropertyController : ControllerBase
    {
        private readonly IPropertyService _propertyService;

        /// <summary>
        /// Creates a new <see cref="PropertyController"/>.
        /// </summary>
        public PropertyController(IPropertyService propertyService)
        {
            _propertyService = propertyService;
        }

        /// <summary>
        /// Returns a paginated list of properties with optional filters.
        /// </summary>
        /// <param name="q">
        /// Query filters: <br/>
        /// • <c>Name</c> (contains, case-insensitive) <br/>
        /// • <c>Address</c> (contains, case-insensitive) <br/>
        /// • <c>MinPrice</c>, <c>MaxPrice</c> <br/>
        /// • <c>Page</c> (1-based, default 1), <c>PageSize</c> (default 10, max 100)
        /// </param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>
        /// A <see cref="PagedResponse{T}"/> with <see cref="PropertyResponse"/> items and total count.
        /// </returns>
        /// <response code="200">The paginated list of properties.</response>
        /// <remarks>
        /// **Examples**:
        /// <code>
        /// GET /api/properties?page=1&amp;pageSize=10
        /// GET /api/properties?name=blue&amp;minPrice=100000&amp;maxPrice=500000
        /// </code>
        /// </remarks>
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

        /// <summary>
        /// Returns a property by its unique identifier.
        /// </summary>
        /// <param name="id">Property Id (GUID).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns><see cref="PropertyResponse"/>.</returns>
        /// <response code="200">The property was found.</response>
        /// <response code="400">The provided Id is invalid.</response>
        /// <response code="404">The property does not exist.</response>
        /// <remarks>
        /// **Example**:
        /// <code>
        /// GET /api/properties/11111111-2222-3333-4444-555555555555
        /// </code>
        /// </remarks>
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

        /// <summary>
        /// Returns a property by its name.
        /// </summary>
        /// <param name="req">Route-bound request with <c>Name</c> field.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns><see cref="PropertyResponse"/>.</returns>
        /// <response code="200">The property was found.</response>
        /// <response code="400">Invalid name.</response>
        /// <response code="404">No property matched the given name.</response>
        /// <remarks>
        /// **Example**:
        /// <code>
        /// GET /api/property/Blue%20House
        /// </code>
        /// </remarks>
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

        /// <summary>
        /// Returns an view of a property (owner, images, traces) by Id.
        /// </summary>
        /// <param name="id">Property Id (GUID).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns><see cref="PropertyDetailResponse"/> including owner and trace info.</returns>
        /// <response code="200">The detailed property was found.</response>
        /// <response code="404">The property does not exist.</response>
        /// <remarks>
        /// **Example**:
        /// <code>
        /// GET /api/properties/11111111-2222-3333-4444-555555555555/detail
        /// </code>
        /// </remarks>
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