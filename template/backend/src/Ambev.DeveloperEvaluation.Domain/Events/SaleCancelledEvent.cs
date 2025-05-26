namespace Ambev.DeveloperEvaluation.Domain.Events;

/// <summary>
/// Event triggered when a sale is cancelled.
/// </summary>
/// <param name="SaleId">The unique identifier of the cancelled sale.</param>
public record SaleCancelledEvent(Guid SaleId) : IDomainEvent
{
    /// <summary>
    /// Timestamp of when the event occurred.
    /// </summary>
    public DateTime OccurredAt { get; } = DateTime.UtcNow;
}