using MillionApi.Application.Services.Owner;

namespace MillionApi.Application.Services.Property
{
    public record PropertyDetailResult(
        Guid Id,
        string Name,
        string Address,
        decimal Price,
        string CodeInternal,
        int Year,
        OwnerResult? Owner,
        string? FirstImageUrl,
        List<PropertyTraceResult> Traces
    );
}