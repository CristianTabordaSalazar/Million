using MillionApi.Contracts.Owner;

namespace MillionApi.Contracts.Property
{
    /// <summary>
    /// Represents a full property detail, including owner info and trace history.
    /// </summary>
    /// <param name="Id">Unique identifier of the property.</param>
    /// <param name="Name">Public display name of the property.</param>
    /// <param name="Address">Physical address of the property.</param>
    /// <param name="Price">Current listed price of the property.</param>
    /// <param name="CodeInternal">Internal reference code used by the system.</param>
    /// <param name="Year">Construction year of the property.</param>
    /// <param name="Owner">Owner information associated with the property.</param>
    /// <param name="FirstImageUrl">URL to the image of the property</param>
    /// <param name="Traces">Historical trace entries.</param>
    public record PropertyDetailResponse(
        Guid Id,
        string Name,
        string Address,
        decimal Price,
        string CodeInternal,
        int Year,
        OwnerResponse Owner,
        string? FirstImageUrl,
        List<PropertyTraceResponse> Traces);
}