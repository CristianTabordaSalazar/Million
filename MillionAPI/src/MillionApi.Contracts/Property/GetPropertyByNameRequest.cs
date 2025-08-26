namespace MillionApi.Contracts.Property
{
    /// <summary>
    /// Request payload to search a property by exact name.
    /// </summary>
    /// <param name="Name">Property name to look up</param>
    public record GetPropertyByNameRequest(
        [System.ComponentModel.DataAnnotations.Required, System.ComponentModel.DataAnnotations.StringLength(100, MinimumLength = 1)]
        string Name);
}