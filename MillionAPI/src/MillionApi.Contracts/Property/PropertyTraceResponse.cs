namespace MillionApi.Contracts.Property
{
    /// <summary>
    /// Represents a trace or history associated with a property.
    /// </summary>
    /// <param name="Id">Unique identifier of the trace record.</param>
    /// <param name="DateSale">Date when the sale or event occurred.</param>
    /// <param name="Name">Label for this trace.</param>
    /// <param name="Value">Monetary value of the trace.</param>
    /// <param name="Tax">Tax amount.</param>
    public record PropertyTraceResponse(
        Guid Id,
        DateTime DateSale,
        string Name,
        decimal Value,
        decimal Tax);
}