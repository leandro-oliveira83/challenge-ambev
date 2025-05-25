namespace Ambev.DeveloperEvaluation.Domain.Events;

/// <summary>
/// Domain event triggered when a sale is updated.
/// </summary>
public record SaleModifiedEvent(Guid SaleId, string SaleNumber, decimal TotalAmount) : IDomainEvent
{
    /// <summary>
    /// Timestamp indicating when the event occurred.
    /// </summary>
    public DateTime OccurredAt { get; } = DateTime.UtcNow;
}