namespace MillionApi.Contracts.Property
{
    public record PropertyResponse(
        Guid Id,
        string Name,
        string Address,
        decimal Price,
        string CodeInternal,
        int Year
    );
}