namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;

/// <summary>
/// API response model for the GetSale operation.
/// </summary>
public class GetSaleResponse
{
    /// <summary>
    /// The unique identifier of the sale.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The sale number used to identify the transaction.
    /// </summary>
    public string SaleNumber { get; set; } = string.Empty;

    /// <summary>
    /// The date when the sale was registered.
    /// </summary>
    public DateTime Date { get; set; }

    /// <summary>
    /// The name of the customer who made the purchase.
    /// </summary>
    public string CustomerName { get; set; } = string.Empty;

    /// <summary>
    /// The name of the branch where the sale was made.
    /// </summary>
    public string BranchName { get; set; } = string.Empty;

    /// <summary>
    /// The total monetary amount of the sale.
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// The list of items included in the sale.
    /// </summary>
    public List<GetSaleItemResponse> Items { get; set; } = [];
}