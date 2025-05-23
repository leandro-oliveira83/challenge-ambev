using Ambev.DeveloperEvaluation.Domain.ValueObjects;

namespace Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;

/// <summary>
/// Represents the response returned after successfully updated product.
/// </summary>
/// <remarks>
/// This response contains the unique identifier of the updated product,
/// which can be used for subsequent operations or reference.
/// </remarks>
public class UpdateProductResult
{
    /// <summary>
    /// Gets or sets the unique identifier of the updated product.
    /// </summary>
    /// <value>A GUID that uniquely identifies the updated product in the system.</value>
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