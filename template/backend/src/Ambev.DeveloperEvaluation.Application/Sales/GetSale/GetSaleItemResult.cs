namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale;

/// <summary>
/// Represents the result of a single item included in a sale item.
/// </summary>
public class GetSaleItemResult
{
    /// <summary>
    /// Gets or sets the product ID being sold.
    /// </summary>
    public Guid ProductId { get; set; }
    
    /// <summary>
    /// Gets or sets the product name.
    /// </summary>
    public string ProductName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the quantity sold.
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Gets or sets the unit price of the item.
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// Gets or sets the discount applied to the item.
    /// </summary>
    public decimal Discount { get; set; }

    /// <summary>
    /// Gets or sets the total amount for the item (already applying discount).
    /// </summary>
    public decimal Total { get; set; }
}