using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.CreateProduct;

/// <summary>
/// Validator for CreateProductRequest that defines validation rules for product creation.
/// </summary>
public class CreateProductRequestValidator : AbstractValidator<CreateProductRequest>
{
    /// <summary>
    /// Initializes a new instance of the CreateProductRequestValidator with defined validation rules.
    /// </summary>
    /// <remarks>
    /// Validation rules include:
    /// The validation includes checking:
    /// - Title: Required, must be between 3 and 100 characters
    /// - Description: Required, must be between 3 and 300 characters
    /// - Category: Required, must be between 3 and 100 characters
    /// - Price: Required, must be greater than zero
    /// - Image: Required, must be between 3 and 100 characters
    /// - Rating: Required, must be not null
    /// </remarks>
    public CreateProductRequestValidator()
    {
        RuleFor(product => product.Title).NotEmpty().Length(3, 100);
        RuleFor(product => product.Description).NotEmpty().Length(3, 300);
        RuleFor(product => product.Category).NotEmpty().Length(3, 100);
        RuleFor(product => product.Price).GreaterThan(0);
        RuleFor(product => product.Image).NotEmpty().Length(3, 100);

        RuleFor(product => product.Rating).NotNull();
        When(p => p.Rating is not null, () =>
        {
            RuleFor(p => p.Rating.Rate).GreaterThanOrEqualTo(0);
            RuleFor(p => p.Rating.Count).GreaterThanOrEqualTo(0);
        });
    }
}