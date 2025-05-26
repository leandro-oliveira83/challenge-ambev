namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

/// <summary>
/// Represents the response returned after successfully updating a new sale.
/// </summary>
/// <remarks>
/// This response contains the unique identifier of the updating sale,
/// as well as metadata such as customer, branch, and total value of the sale.
/// </remarks>
public class UpdateSaleResult
{
    /// <summary>
    /// Gets or sets the unique identifier of the updating sale.
    /// </summary>
    /// <value>A GUID that uniquely identifies the updated sale in the system.</value>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the unique sale number.
    /// </summary>
    public string SaleNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the date the sale was created.
    /// </summary>
    public DateTime Date { get; set; }

    /// <summary>
    /// Gets or sets the customer name.
    /// </summary>
    public string CustomerName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the branch name.
    /// </summary>
    public string BranchName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the total sale amount.
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Gets or sets the list of items included in the sale.
    /// </summary>
    public List<UpdateSaleItemResult> Items { get; set; } = [];
}