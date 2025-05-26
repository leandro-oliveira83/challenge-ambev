using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.TestData;

public class UpdateSaleHandlerTestData
{
    private static readonly Faker<UpdateSaleCommand> updateSaleFaker = new Faker<UpdateSaleCommand>()
        .RuleFor(s => s.Id, f => Guid.NewGuid())
        .RuleFor(s => s.SaleNumber, f => f.Random.AlphaNumeric(10).ToUpper())
        .RuleFor(s => s.Date, f => f.Date.Recent())
        .RuleFor(s => s.CustomerId, f => f.Random.Guid().ToString())
        .RuleFor(s => s.CustomerName, f => f.Name.FullName())
        .RuleFor(s => s.BranchId, f => f.Random.Guid().ToString())
        .RuleFor(s => s.BranchName, f => f.Company.CompanyName())
        .RuleFor(s => s.Items, f => new List<UpdateSaleItemCommand>
        {
            new UpdateSaleItemCommand
            {
                ProductId = Guid.NewGuid(),
                ProductName = "Test Product",
                Quantity = 10,
                UnitPrice = 99.99m
            }
        });

    public static UpdateSaleCommand GenerateValidCommand()
    {
        return updateSaleFaker.Generate();
    }

    public static UpdateSaleCommand GenerateCommandWithMultipleItems(int count)
    {
        var command = updateSaleFaker.Generate();
        command.Items = Enumerable.Range(0, count).Select(_ => new UpdateSaleItemCommand
        {
            ProductId = Guid.NewGuid(),
            ProductName = $"Product {_ + 1}",
            Quantity = 5,
            UnitPrice = 20.00m
        }).ToList();
        return command;
    }
    
    public static Sale CreateSaleByCommand(UpdateSaleCommand command)
    {
        var sale = new Sale(
            saleNumber: command.SaleNumber,
            date: command.Date,
            customerId: command.CustomerId,
            customerName: command.CustomerName,
            branchId: command.BranchId,
            branchName: command.BranchName
        );

        foreach (var item in command.Items)
        {
            sale.AddItem(item.ProductId, item.ProductName, item.Quantity, item.UnitPrice);
        }

        return sale;
    }
}