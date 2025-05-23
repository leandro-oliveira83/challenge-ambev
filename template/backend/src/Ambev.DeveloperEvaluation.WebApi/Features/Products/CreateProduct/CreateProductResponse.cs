using Ambev.DeveloperEvaluation.Domain.ValueObjects;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.CreateProduct;

/// <summary>
/// API response model for CreateProduct operation
/// </summary>
public class CreateProductResponse
{
    /// <summary>
    /// The unique identifier of the created product
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// The product's title
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// The product's price.
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// The product's description.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// The product's name.
    /// </summary>
    public string Category { get; set; } = string.Empty;
    
    /// <summary>
    /// The product's image.
    /// </summary>
    public string Image { get; set; } = string.Empty;

    /// <summary>
    /// The product's rating.
    /// </summary>
    public Rating Rating { get; set; } = default!;
}