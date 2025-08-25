namespace MillionApi.Contracts.Property
{
    public sealed record PropertiesQuery(
        string? Name,
        string? Address,
        decimal? MinPrice,
        decimal? MaxPrice,
        int Page = 1,
        int PageSize = 10
    );
}