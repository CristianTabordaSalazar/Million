namespace MillionApi.Domain.Entities
{
    public class PropertyTrace
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime DateSale { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Value { get; set; }
        public decimal Tax { get; set; }
        public Guid PropertyId { get; set; }
    }
}