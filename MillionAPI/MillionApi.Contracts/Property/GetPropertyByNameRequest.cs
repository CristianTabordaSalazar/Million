namespace MillionApi.Contracts.Property
{
    public record GetPropertyByNameRequest(
        [System.ComponentModel.DataAnnotations.Required, System.ComponentModel.DataAnnotations.StringLength(100, MinimumLength = 1)]
        string Name);
}