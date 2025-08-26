namespace MillionApi.Contracts.Common
{
    /// <summary>
    /// Represents a paged response with items and total count.
    /// </summary>
    /// <param name="Items">List of items in the page.</param>
    /// <param name="Total">Total number of items available.</param>
    public sealed record PagedResponse<T>(
        IReadOnlyList<T> Items,
        long Total);
}
