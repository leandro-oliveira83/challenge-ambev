using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

/// <summary>
/// Command for creating a new sale item.
/// </summary>
/// <remarks>
/// This command captures all necessary data for registering a sale, including customer and branch info,
/// sale items, and sale metadata. It implements <see cref="IRequest{TResponse}"/> to initiate
/// the creation process and return a <see cref="CreateSaleResult"/>.
/// Validation is handled by <see cref="CreateSaleCommandValidator"/>.
/// </remarks>
public class CreateSaleItemCommand: IRequest<CreateSaleItemResult>
{
    /// <summary>
    /// Gets or sets the product ID.
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Gets or sets the product name.
    /// </summary>
    public string ProductName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the quantity of the product.
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Gets or sets the unit price of the product.
    /// </summary>
    public decimal UnitPrice { get; set; }
}