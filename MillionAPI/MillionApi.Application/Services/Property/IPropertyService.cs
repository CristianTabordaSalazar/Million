namespace MillionApi.Application.Services.Property
{
    public interface IPropertyService
    {
        Task<PropertyResult?> GetPropertyByNameAsync(string name, CancellationToken ct = default);
    }
}