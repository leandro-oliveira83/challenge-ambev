namespace Ambev.DeveloperEvaluation.Domain.Events;

/// <summary>
/// Domain event triggered when a sale is created.
/// </summary>
public record SaleCreatedEvent(Guid SaleId, string SaleNumber, decimal TotalAmount) : IDomainEvent
{
    /// <summary>
    /// Timestamp indicating when the event occurred.
    /// </summary>
    public DateTime OccurredAt { get; } = DateTime.UtcNow;
}