namespace MillionApi.Contracts.Common
{
    public sealed record PagedResponse<T>(
        IReadOnlyList<T> Items,
        long Total);
}
