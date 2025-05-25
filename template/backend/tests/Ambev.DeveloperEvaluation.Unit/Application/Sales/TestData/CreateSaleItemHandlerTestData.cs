using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.TestData;
public static class CreateSaleItemHandlerTestData
{
    private static readonly Faker<CreateSaleItemCommand> itemFaker = new Faker<CreateSaleItemCommand>()
        .RuleFor(i => i.ProductId, f => f.Random.Guid())
        .RuleFor(i => i.ProductName, f => f.Commerce.ProductName())
        .RuleFor(i => i.Quantity, f => f.Random.Int(1, 20))
        .RuleFor(i => i.UnitPrice, f => f.Random.Decimal(10, 200));

    public static CreateSaleItemCommand GenerateValidItem()
    {
        return itemFaker.Generate();
    }

    public static List<CreateSaleItemCommand> GenerateValidList(int count = 2)
    {
        return itemFaker.Generate(count);
    }
}