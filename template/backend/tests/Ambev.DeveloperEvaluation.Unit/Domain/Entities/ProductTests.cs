using Xunit;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using FluentAssertions;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

/// <summary>
/// Unit tests for the <see cref="Product"/> domain entity.
/// Validates business rules, state changes, and integrity of the entity.
/// </summary>
public class ProductTests
{
    /// <summary>
    /// Should update product properties and set UpdatedAt when calling the Change method.
    /// </summary>
    [Fact(DisplayName = "When product information is changed Then UpdatedAt should be set and fields updated")]
    public void Change_ShouldUpdateProductFields_AndSetUpdatedAt()
    {
        // Arrange
        var product = ProductTestData.GenerateValid();

        // Act
        product.Change(
            "Title", 
            1, 
            "Description", 
            "Category", 
            "https://image.jpg", 
            new()
            );

        // Assert
        product.Title.Should().Be("Title");
        product.Description.Should().Be("Description");
        product.Category.Should().Be("Category");
        product.Price.Should().Be(1);
        product.Image.Should().Be("https://image.jpg");
        product.UpdatedAt.Should().NotBeNull();
    }
    
    /// <summary>
    /// Should return validation errors when product is in an invalid state.
    /// </summary>
    [Fact(DisplayName = "When product has invalid data Then validation should fail with errors")]
    public void Validate_ShouldFail_WhenProductHasInvalidData()
    {
        // Arrange
        var product = new Product();

        // Act
        var result = product.Validate();

        // Assert
        Assert.False(result.IsValid);
        Assert.NotEmpty(result.Errors);
    }
}