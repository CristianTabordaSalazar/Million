namespace MillionApi.Application.Services.Property
{
    public record PropertyTraceResult(
        Guid Id,
        DateTime DateSale,
        string Name,
        decimal Value,
        decimal Tax);
}