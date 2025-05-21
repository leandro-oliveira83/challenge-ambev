using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Validation;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents a product in the system.
/// This entity follows domain-driven design principles and includes business rules validation.
/// </summary>
public class Product : BaseEntity
{ 
    /// <summary>
    /// Gets the product's title.
    /// Must not be null or empty.
    /// </summary>
    public string Title { get; private set; }

    /// <summary>
    /// Gets the product's price.
    /// Must be greater than zero.
    /// </summary>
    public decimal Price { get; private set; }

    /// <summary>
    /// Gets the product's description.
    /// </summary>
    public string Description { get; private set; }
    
    /// <summary>
    /// Gets the product's category.
    /// </summary>
    public string Category { get; set; }

    /// <summary>
    /// Gets the product's image.
    /// </summary>
    public string Image { get; private set; }
    
    /// <summary>
    /// Gets the product's rating.
    /// </summary>
    public Rating Rating { get; private set; }

    /// <summary>
    /// Gets the date and time when the product was created.
    /// </summary>
    public DateTime CreatedAt { get; init; }

    /// <summary>
    /// Gets the date and time of the last update to the product's information.
    /// </summary>
    public DateTime? UpdatedAt { get; private set; }

    /// <summary>
    /// Initializes a new instance of the Product class.
    /// </summary>
    public Product()
    {
        CreatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Performs validation of the user entity using the ProductValidator rules.
    /// </summary>
    /// <returns>
    /// A <see cref="ValidationResultDetail"/> containing:
    /// - IsValid: Indicates whether all validation rules passed
    /// - Errors: Collection of validation errors if any rules failed
    /// </returns>
    /// <remarks>
    /// <listheader>The validation includes checking:</listheader>
    /// <list type="bullet">Title length</list>
    /// <list type="bullet">Description length</list>
    /// <list type="bullet">Category length</list>
    /// <list type="bullet">Image length</list>
    /// <list type="bullet">Price amount</list>
    /// <list type="bullet">Rating</list>
    /// </remarks>
    public ValidationResultDetail Validate()
    {
        var validator = new ProductValidator();
        var result = validator.Validate(this);
        return new ValidationResultDetail
        {
            IsValid = result.IsValid,
            Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
        };
    }
    
    /// <summary>
    /// Change product.
    /// </summary>
    /// <param name="title">Title of product.</param>
    /// <param name="description">Description of product.</param>
    /// <param name="image">Image of product.</param>
    /// <param name="price">Price of product.</param>
    /// <param name="rating">Rating of product.</param>
    public void Change(
        string title,
        decimal price,
        string description,
        string category,
        string image,
        Rating rating)
    {
        Title = title;
        Price = price;
        Description = description;
        Category = category;
        Image = image;
        Rating = rating;
        UpdatedAt = DateTime.UtcNow;
    }
}