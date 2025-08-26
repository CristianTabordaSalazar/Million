namespace MillionApi.Contracts.Owner
{
    public record OwnerResponse(
        Guid Id,
        string Name,
        string Address,
        string Photo,
        DateTime DateOfBirth
    );
}