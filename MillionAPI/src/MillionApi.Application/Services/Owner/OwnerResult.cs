namespace MillionApi.Application.Services.Owner
{
    public record OwnerResult(
        Guid Id,
        string Name,
        string Address,
        string Photo,
        DateTime DateOfBirth);
}