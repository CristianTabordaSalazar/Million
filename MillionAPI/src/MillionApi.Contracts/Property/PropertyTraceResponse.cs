namespace MillionApi.Contracts.Property
{
    public record PropertyTraceResponse(
        Guid Id,
        DateTime DateSale,
        string Name,
        decimal Value,
        decimal Tax
    );
}