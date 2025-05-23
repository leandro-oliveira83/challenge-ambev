using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;

public class ProductTestData
{
    private static readonly Faker<Product> ProductFaker = new Faker<Product>()
        .RuleFor(c => c.Id, f => f.Random.Guid())
        .RuleFor(u => u.Title, f => f.Lorem.Sentence(4))
        .RuleFor(u => u.Description, f => f.Lorem.Sentences(2))
        .RuleFor(p => p.Category, f => f.Lorem.Sentences(2))
        .RuleFor(u => u.Image, f => f.Internet.Url())
        .RuleFor(u => u.Price, f => f.Random.Decimal(min: 1, max: 100.00M));

    public static Product GenerateValid()
    {
        return ProductFaker.Generate();
    }
}