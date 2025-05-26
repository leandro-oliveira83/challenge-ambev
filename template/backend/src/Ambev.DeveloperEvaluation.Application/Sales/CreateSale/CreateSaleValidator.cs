using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

/// <summary>
/// Validator for CreateSaleCommand that defines validation rules for sale creation.
/// </summary>
public class CreateSaleCommandValidator : AbstractValidator<CreateSaleCommand>
{
    /// <summary>
    /// Initializes a new instance of the CreateSaleCommandValidator with defined validation rules.
    /// </summary>
    public CreateSaleCommandValidator()
    {
        RuleFor(s => s.SaleNumber)
            .NotEmpty()
            .MaximumLength(50);

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
            .WithMessage("Sale must contain at least one item.")
            .Must(HaveUniqueProductIds)
            .WithMessage("Duplicate product entries are not allowed.");

        When(s => s.Items != null, () =>
        {
            RuleForEach(s => s.Items).ChildRules(item =>
            {
                item.RuleFor(i => i.ProductId).NotEmpty();
                item.RuleFor(i => i.ProductName).NotEmpty().MaximumLength(150);
                item.RuleFor(i => i.Quantity)
                    .GreaterThan(0)
                    .LessThanOrEqualTo(20)
                    .WithMessage("Cannot sell more than 20 identical items.");
        
                item.RuleFor(i => i.UnitPrice)
                    .GreaterThan(0)
                    .WithMessage("Unit price must be greater than 0.");
            });
        });
    }

    /// <summary>
    /// Verifica se há produtos duplicados no array de items.
    /// </summary>
    private static bool HaveUniqueProductIds(IEnumerable<CreateSaleItemCommand> items)
    {
        return items.Select(i => i.ProductId).Distinct().Count() == items.Count();
    }
}