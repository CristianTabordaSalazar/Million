namespace MillionApi.Application.Services.Property
{
    public record PropertyResult(
        Guid Id,
        string Name,
        string Address,
        decimal Price,
        string CodeInternal,
        int Year
    );
}