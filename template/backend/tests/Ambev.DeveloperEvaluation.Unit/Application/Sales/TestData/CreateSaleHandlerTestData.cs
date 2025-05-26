using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.TestData;

public static class CreateSaleHandlerTestData
{
    private static readonly Faker<CreateSaleCommand> saleFaker = new Faker<CreateSaleCommand>()
        .RuleFor(s => s.SaleNumber, f => f.Random.AlphaNumeric(10).ToUpper())
        .RuleFor(s => s.Date, f => f.Date.Recent())
        .RuleFor(s => s.CustomerId, f => f.Random.Guid().ToString())
        .RuleFor(s => s.CustomerName, f => f.Name.FullName())
        .RuleFor(s => s.BranchId, f => f.Random.Guid().ToString())
        .RuleFor(s => s.BranchName, f => f.Company.CompanyName())
        .RuleFor(s => s.Items, f => CreateSaleItemHandlerTestData.GenerateValidList());

    public static CreateSaleCommand GenerateValidCommand()
    {
        return saleFaker.Generate();
    }
    
    public static CreateSaleCommand GenerateValidCommandWithQuantity(int quantity)
    {
        return new CreateSaleCommand
        {
            SaleNumber = Guid.NewGuid().ToString("N")[..10],
            Date = DateTime.UtcNow,
            CustomerId = Guid.NewGuid().ToString(),
            CustomerName = "Customer Test",
            BranchId = Guid.NewGuid().ToString(),
            BranchName = "Branch Test",
            Items = new List<CreateSaleItemCommand>
            {
                new()
                {
                    ProductId = Guid.NewGuid(),
                    ProductName = "Beer",
                    Quantity = quantity,
                    UnitPrice = 10
                }
            }
        };
    }
}