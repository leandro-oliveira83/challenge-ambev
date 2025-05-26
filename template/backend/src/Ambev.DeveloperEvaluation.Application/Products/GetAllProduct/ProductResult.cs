using Ambev.DeveloperEvaluation.Domain.ValueObjects;

namespace Ambev.DeveloperEvaluation.Application.Products.GetAllProduct;

/// <summary>
/// Response model for ListProduct operations
/// </summary>
public class ProductResult
{
    /// <summary>
    /// The unique identifier of the product
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the product's name.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the product's description.
    /// </summary>
    public string Description { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the product's category name.
    /// </summary>
    public string Category { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the product's image.
    /// </summary>
    public string Image { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the product's price.
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Gets or sets the product's rating.
    /// </summary>
    public Rating Rating { get; set; } = default!;
}