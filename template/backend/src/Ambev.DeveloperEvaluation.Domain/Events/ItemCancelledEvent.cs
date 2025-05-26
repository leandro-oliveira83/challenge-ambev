namespace Ambev.DeveloperEvaluation.Domain.Events;

/// <summary>
/// Domain event triggered when a sale item is cancelled.
/// </summary>
public record ItemCancelledEvent(Guid SaleId, Guid ProductId, string ProductName) : IDomainEvent
{
    /// <summary>
    /// Timestamp indicating when the event occurred.
    /// </summary>
    public DateTime OccurredAt { get; } = DateTime.UtcNow;
}