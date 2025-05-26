using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Products.CreateProduct;

/// <summary>
/// Validator for CreateProductCommandValidator that defines validation rules for product creating command.
/// </summary>
public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    /// <summary>
    /// Initializes a new instance of the CreateProductCommandValidator with defined validation rules.
    /// </summary>
    /// <remarks>
    /// Validation rules include:
    /// - Title: Required, must be between 3 and 100 characters
    /// - Description: Required, must be between 3 and 300 characters
    /// - Category: Required, must be between 3 and 100 characters
    /// - Price: Required, must be greater than zero
    /// - Image: Required, must be between 3 and 100 characters
    /// - Rating: Required, must be not null
    /// </remarks>
    public CreateProductCommandValidator()
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