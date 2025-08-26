namespace MillionApi.Contracts.Property
{
    /// <summary>
    /// Represents a property item.
    /// </summary>
    /// <param name="Id">Unique identifier of the property.</param>
    /// <param name="Name">Public display name of the property.</param>
    /// <param name="Address">Physical address of the property.</param>
    /// <param name="Price">Current listed price of the property.</param>
    /// <param name="CodeInternal">Internal reference code used by the system.</param>
    /// <param name="Year">Construction year of the property.</param>
    public record PropertyResponse(
        Guid Id,
        string Name,
        string Address,
        decimal Price,
        string CodeInternal,
        int Year);
}