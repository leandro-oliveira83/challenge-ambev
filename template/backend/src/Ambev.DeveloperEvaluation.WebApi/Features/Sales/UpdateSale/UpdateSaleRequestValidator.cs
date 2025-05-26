using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;

/// <summary>
/// Validator for UpdateSaleRequest that defines validation rules for sale updating.
/// </summary>
public class UpdateSaleRequestValidator : AbstractValidator<UpdateSaleRequest>
{
    /// <summary>
    /// Initializes a new instance of the UpdateSaleRequestValidator with defined validation rules.
    /// </summary>
    /// <remarks>
    /// Validation rules include:
    /// - Id: Required, must be empty
    /// - SaleNumber: Required, 3 to 50 characters
    /// - Date: Must not be in the future
    /// - CustomerId & CustomerName: Required
    /// - BranchId & BranchName: Required
    /// - Items: Required and must contain at least 1 valid item
    /// - Each item: ProductId and ProductName required, Quantity 1-20, UnitPrice > 0
    /// </remarks>
    public UpdateSaleRequestValidator()
    {
        RuleFor(product => product.Id).NotEmpty();
        RuleFor(s => s.SaleNumber)
            .NotEmpty()
            .Length(3, 50);

        RuleFor(s => s.Date)
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("Sale date cannot be in the future.");

        RuleFor(s => s.CustomerId)
            .NotEmpty();

        RuleFor(s => s.CustomerName)
            .NotEmpty()
            .MaximumLength(150);

        RuleFor(s => s.BranchId)
            .NotEmpty();

        RuleFor(s => s.BranchName)
            .NotEmpty()
            .MaximumLength(150);

        RuleFor(s => s.Items)
            .NotNull()
            .NotEmpty()
            .WithMessage("Sale must contain at least one item.");
        
        When(s => s.Items != null, () =>
        {
            RuleForEach(s => s.Items).ChildRules(item =>
            {
                item.RuleFor(i => i.ProductId)
                    .NotEmpty();
        
                item.RuleFor(i => i.ProductName)
                    .NotEmpty()
                    .MaximumLength(150);
        
                item.RuleFor(i => i.Quantity)
                    .GreaterThan(0)
                    .LessThanOrEqualTo(20)
                    .WithMessage("Quantity must be between 1 and 20.");
        
                item.RuleFor(i => i.UnitPrice)
                    .GreaterThan(0)
                    .WithMessage("Unit price must be greater than 0.");
            });
        });
    }
}