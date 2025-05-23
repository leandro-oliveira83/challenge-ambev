using Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.Products.TestData;

/// <summary>
/// Provides methods for generating test data using the Bogus library.
/// This class centralizes all test data generation to ensure consistency
/// across test cases and provide both valid and invalid data scenarios.
/// </summary>
public class UpdateProductHandlerTestData
{
    /// <summary>
    /// Configures the Faker to generate valid Product entities.
    /// The generated products will have valid:
    /// - Id
    /// - Title
    /// - Category
    /// - Price
    /// - Image
    /// - Rating
    /// </summary>
    private static readonly Faker<UpdateProductCommand> createProductHandlerFaker = new Faker<UpdateProductCommand>()
        .RuleFor(u => u.Id, f => f.Random.Guid())
        .RuleFor(u => u.Title, f => f.Lorem.Sentence(4))
        .RuleFor(u => u.Description, f => f.Lorem.Sentences(2))
        .RuleFor(p => p.Category, f => f.Commerce.Categories(1).First())
        .RuleFor(u => u.Price, f => f.Random.Decimal(min: 1, max: 2000.00M))
        .RuleFor(u => u.Image, f => f.Internet.Url())
        .RuleFor(p => p.Rating, f => new DeveloperEvaluation.Domain.ValueObjects.Rating
        {
            Rate = f.Random.Int(min: 0, max: 100),
            Count = f.Random.Int(min: 0, max: 1000),
        });
    
    /// <summary>
    /// Generates a valid Product entity with randomized data.
    /// The generated user will have all properties populated with valid values
    /// that meet the system's validation requirements.
    /// </summary>
    /// <returns>A valid Product entity with randomly generated data.</returns>
    public static UpdateProductCommand GenerateValidCommand()
    {
        return createProductHandlerFaker.Generate();
    }
    
    public static Product CreateProductByCommand(UpdateProductCommand command)
    {
        var faker = new Faker<Product>()
            .RuleFor(u => u.Id, command.Id)
            .RuleFor(u => u.Title, command.Title)
            .RuleFor(u => u.Description, command.Description)
            .RuleFor(p => p.Category, command.Category)
            .RuleFor(u => u.Price, command.Price)
            .RuleFor(u => u.Image, command.Image);
        return faker.Generate(1).First();
    }
}