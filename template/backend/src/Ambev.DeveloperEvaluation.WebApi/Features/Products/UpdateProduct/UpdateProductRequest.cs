using Ambev.DeveloperEvaluation.Domain.ValueObjects;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.UpdateProduct;

/// <summary>
/// Represents a request to update a product in the system.
/// </summary>
public class UpdateProductRequest
{
    /// <summary>
    /// The unique identifier of the product to update.
    /// </summary>
    public Guid Id { get; private set; }
    
    /// <summary>
    /// Gets or sets the title. Must be unique and contain only valid characters.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the price.
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Gets or sets the description.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the category name.
    /// </summary>
    public string Category { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the image.
    /// </summary>
    public string Image { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the rating.
    /// </summary>
    public Rating Rating { get; set; } = default!;
    
    /// <summary>
    /// Set id to request.
    /// </summary>
    /// <param name="id">Identifier</param>
    /// <returns>Instance UpdateProductRequest of request.</returns>
    public UpdateProductRequest SetId(Guid id)
    {
        Id = id;
        return this;
    }
}