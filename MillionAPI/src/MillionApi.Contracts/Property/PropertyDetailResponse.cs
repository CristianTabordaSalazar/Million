using MillionApi.Contracts.Owner;

namespace MillionApi.Contracts.Property
{
    public record PropertyDetailResponse(
        Guid Id,
        string Name,
        string Address,
        decimal Price,
        string CodeInternal,
        int Year,
        OwnerResponse Owner,
        string? FirstImageUrl,
        List<PropertyTraceResponse> Traces
    );
}