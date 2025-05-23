using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents an item in a sale, including product info, quantity, pricing and discount rules.
/// </summary>
public class SaleItem : BaseEntity
{
    /// <summary>
    /// Gets the ID of the sale this item belongs to.
    /// </summary>
    public Guid SaleId { get; private set; } 
    
    /// <summary>
    /// Gets the ID of the associated product.
    /// </summary>
    public Guid ProductId { get; private set; }

    /// <summary>
    /// Gets the name of the associated product.
    /// </summary>
    public string ProductName { get; private set; }

    /// <summary>
    /// Gets the quantity of items sold.
    /// </summary>
    public int Quantity { get; private set; }

    /// <summary>
    /// Gets the unit price of the product.
    /// </summary>
    public decimal UnitPrice { get; private set; }

    /// <summary>
    /// Gets the applied discount as a percentage (e.g., 0.1 for 10%).
    /// </summary>
    public decimal Discount { get; private set; }

    /// <summary>
    /// Gets the total amount for this item (quantity * price * discount).
    /// </summary>
    public decimal Total => Quantity * UnitPrice * (1 - Discount);

    /// <summary>
    /// Gets whether the item has been cancelled.
    /// </summary>
    public bool IsCancelled { get; private set; }

    /// <summary>
    /// Initializes a new instance of SaleItem.
    /// </summary>
    public SaleItem(Guid productId, string productName, int quantity, decimal unitPrice, decimal discount)
    {
        ProductId = productId;
        ProductName = productName;
        Quantity = quantity;
        UnitPrice = unitPrice;
        Discount = discount;
        IsCancelled = false;
    }

    /// <summary>
    /// Cancels this item from the sale.
    /// </summary>
    public void Cancel()
    {
        if (IsCancelled)
            return;

        IsCancelled = true;
    }
}