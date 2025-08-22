namespace MillionApi.Domain.Entities
{
    public class PropertyImage
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Url { get; set; } = string.Empty;
        public bool IsEnabled { get; set; } = true;
        public Guid PropertyId { get; set; }
    }
}