namespace MillionApi.Contracts.Owner
{
    /// <summary>
    /// Represents the public information of a property owner.
    /// </summary>
    /// <param name="Id">Unique identifier of the owner.</param>
    /// <param name="Name">Full name of the owner.</param>
    /// <param name="Address">Address registered for the owner.</param>
    /// <param name="Photo">Public URL to the owner's profile photo.</param>
    /// <param name="DateOfBirth">Date of birth of the owner</param>
    public record OwnerResponse(
        Guid Id,
        string Name,
        string Address,
        string Photo,
        DateTime DateOfBirth
    );
}