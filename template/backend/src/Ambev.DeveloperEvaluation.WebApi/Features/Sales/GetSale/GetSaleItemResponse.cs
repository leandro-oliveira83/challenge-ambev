namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;

/// <summary>
/// API model representing an item in the sale response.
/// </summary>
public class GetSaleItemResponse
{
    /// <summary>
    /// Gets or sets the product ID being sold.
    /// </summary>
    public Guid ProductId { get; set; }
    
    /// <summary>
    /// The product name associated with this item.
    /// </summary>
    public string ProductName { get; set; } = string.Empty;

    /// <summary>
    /// The quantity of the product sold.
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// The price per unit of the product.
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// The discount applied to the item (as percentage, e.g., 0.1 for 10%).
    /// </summary>
    public decimal Discount { get; set; }

    /// <summary>
    /// The total amount charged for this item (after discount).
    /// </summary>
    public decimal Total { get; set; }
}