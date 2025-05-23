using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Validation;
using System.Collections.ObjectModel;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents a sale transaction within the system.
/// Includes customer and branch info, product items, and total calculation.
/// </summary>
public class Sale : BaseEntity
{
    /// <summary>
    /// Gets the unique sale number.
    /// </summary>
    public string SaleNumber { get; private set; }

    /// <summary>
    /// Gets the date and time the sale was created.
    /// </summary>
    public DateTime Date { get; private set; }

    /// <summary>
    /// Gets the customer ID (external identity).
    /// </summary>
    public string CustomerId { get; private set; }

    /// <summary>
    /// Gets the customer name (denormalized).
    /// </summary>
    public string CustomerName { get; private set; }

    /// <summary>
    /// Gets the branch ID (external identity).
    /// </summary>
    public string BranchId { get; private set; }

    /// <summary>
    /// Gets the branch name (denormalized).
    /// </summary>
    public string BranchName { get; private set; }

    /// <summary>
    /// Gets whether the sale has been cancelled.
    /// </summary>
    public bool IsCancelled { get; private set; }

    /// <summary>
    /// Gets the collection of items in this sale.
    /// </summary>
    public ICollection<SaleItem> Items { get; private set; } = new Collection<SaleItem>();

    /// <summary>
    /// Gets the total amount for the sale.
    /// </summary>
    public decimal TotalAmount => Items.Sum(item => item.Total);

    /// <summary>
    /// Gets the creation timestamp.
    /// </summary>
    public DateTime CreatedAt { get; init; }

    /// <summary>
    /// Gets the update timestamp.
    /// </summary>
    public DateTime? UpdatedAt { get; private set; }

    /// <summary>
    /// Initializes a new instance of the Sale class.
    /// </summary>
    public Sale()
    {
        CreatedAt = DateTime.UtcNow;
        IsCancelled = false;
    }

    /// <summary>
    /// Initializes a sale with basic information.
    /// </summary>
    public Sale(string saleNumber, DateTime date, string customerId, string customerName, string branchId, string branchName)
        : this()
    {
        SaleNumber = saleNumber;
        Date = date;
        CustomerId = customerId;
        CustomerName = customerName;
        BranchId = branchId;
        BranchName = branchName;
    }

    /// <summary>
    /// Adds an item to the sale and applies discount rules based on quantity.
    /// </summary>
    /// <param name="productId">Product ID</param>
    /// <param name="productName">Product name</param>
    /// <param name="quantity">Quantity of product</param>
    /// <param name="unitPrice">Unit price</param>
    public void AddItem(Guid productId, string productName, int quantity, decimal unitPrice)
    {
        if (quantity > 20)
            throw new InvalidOperationException("Cannot sell more than 20 identical items.");

        decimal discount = 0;
        if (quantity >= 10)
            discount = 0.20m;
        else if (quantity >= 4)
            discount = 0.10m;

        var item = new SaleItem(productId, productName, quantity, unitPrice, discount);
        Items.Add(item);
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Cancels the entire sale.
    /// </summary>
    public void Cancel()
    {
        if (IsCancelled)
            return;

        IsCancelled = true;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Changes customer and branch info.
    /// </summary>
    public void UpdateCustomer(string customerId, string customerName)
    {
        CustomerId = customerId;
        CustomerName = customerName;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Validates the sale entity.
    /// </summary>
    /// <returns>A validation result detail object.</returns>
    public ValidationResultDetail Validate()
    {
        var validator = new SaleValidator();
        var result = validator.Validate(this);
        return new ValidationResultDetail
        {
            IsValid = result.IsValid,
            Errors = result.Errors.Select(e => (ValidationErrorDetail)e)
        };
    }
}