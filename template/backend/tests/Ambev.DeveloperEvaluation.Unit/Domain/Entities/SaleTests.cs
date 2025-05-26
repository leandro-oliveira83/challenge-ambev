using Xunit;
using FluentAssertions;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

/// <summary>
/// Unit tests for the <see cref="Sale"/> domain entity.
/// Validates business rules, state changes, and integrity of the entity.
/// </summary>
public class SaleTests
{
    /// <summary>
    /// Tests that a discount of 10% is applied when quantity is between 4 and 9.
    /// </summary>
    [Fact(DisplayName = "Given valid item quantity Then should apply correct discount")]
    public void AddItem_ShouldApplyCorrectDiscount()
    {
        var sale = new Sale("S123", DateTime.UtcNow, "cust1", "John Doe", "br001", "Main");

        sale.AddItem(Guid.NewGuid(), "Item A", 5, 100);

        var item = sale.Items.First();
        item.Discount.Should().Be(0.10m);
        item.Total.Should().Be(5 * 100 * 0.9m);
    }
    
    /// <summary>
    /// Tests that an exception is thrown when adding an item with quantity over 20.
    /// </summary>
    [Fact(DisplayName = "Given quantity over 20 Then should throw exception")]
    public void AddItem_QuantityAboveLimit_ShouldThrow()
    {
        var sale = new Sale("S123", DateTime.UtcNow, "cust1", "John Doe", "br001", "Main");

        var act = () => sale.AddItem(Guid.NewGuid(), "Item A", 21, 100);

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Cannot sell more than 20 identical items.");
    }
    
    /// <summary>
    /// Tests that the sale is correctly marked as cancelled.
    /// </summary>
    [Fact(DisplayName = "When canceling sale Then IsCancelled should be true")]
    public void Cancel_ShouldMarkSaleAsCancelled()
    {
        var sale = new Sale("S123", DateTime.UtcNow, "cust1", "John Doe", "br001", "Main");

        sale.Cancel();

        sale.IsCancelled.Should().BeTrue();
        sale.UpdatedAt.Should().NotBeNull();
    }
    
    /// <summary>
    /// Tests that cancelled items are excluded from the total amount calculation.
    /// </summary>
    [Fact(DisplayName = "When calculating total Then cancelled items should be ignored")]
    public void TotalAmount_ShouldIgnoreCancelledItems()
    {
        var sale = new Sale("S123", DateTime.UtcNow, "cust1", "John Doe", "br001", "Main");

        sale.AddItem(Guid.NewGuid(), "Active", 4, 50);       // 10% desconto
        sale.AddItem(Guid.NewGuid(), "ToCancel", 5, 50);     // 10% desconto

        var itemToCancel = sale.Items.Last();
        itemToCancel.Cancel();

        sale.TotalAmount.Should().Be(4 * 50 * 0.9m);
    }
    
    /// <summary>
    /// Tests that customer information is updated correctly.
    /// </summary>
    [Fact(DisplayName = "When updating customer info Then fields should change")]
    public void UpdateCustomer_ShouldUpdateFields()
    {
        var sale = new Sale("S123", DateTime.UtcNow, "cust1", "John Doe", "br001", "Main");

        sale.UpdateCustomer("newCust", "Jane Smith");

        sale.CustomerId.Should().Be("newCust");
        sale.CustomerName.Should().Be("Jane Smith");
    }
}