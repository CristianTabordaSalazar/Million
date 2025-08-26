namespace MillionApi.Contracts.Property
{
    /// <summary>
    /// Query parameters to filter and paginate properties.
    /// </summary>
    /// <param name="Name">Optional case-insensitive filter applied to property name.</param>
    /// <param name="Address">Optional case-insensitive filter applied to property address.</param>
    /// <param name="MinPrice">Optional minimum price (inclusive).</param>
    /// <param name="MaxPrice">Optional maximum price (inclusive).</param>
    /// <param name="Page">1-based page number. Defaults to 1.</param>
    /// <param name="PageSize">Page size. Defaults to 10.</param>
    public sealed record PropertiesQuery(
        string? Name,
        string? Address,
        decimal? MinPrice,
        decimal? MaxPrice,
        int Page = 1,
        int PageSize = 10);
}