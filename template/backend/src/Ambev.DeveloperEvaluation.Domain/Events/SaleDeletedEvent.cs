namespace Ambev.DeveloperEvaluation.Domain.Events;

/// <summary>
/// Event triggered when a sale is deleted.
/// </summary>
/// <param name="SaleId">The unique identifier of the deleted sale.</param>
public record SaleDeletedEvent(Guid SaleId) : IDomainEvent
{
    /// <summary>
    /// Timestamp of when the event occurred.
    /// </summary>
    public DateTime OccurredAt { get; } = DateTime.UtcNow;
}