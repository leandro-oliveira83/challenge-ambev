using Ambev.DeveloperEvaluation.Application.Products.DeleteProduct;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using FluentAssertions;
using FluentValidation;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Products;

/// <summary>
/// Unit tests for validating the behavior of the <see cref="DeleteProductHandler"/>.
/// </summary>
public class DeleteProductHandlerTests
{
    private readonly IProductRepository _productRepository;
    private readonly DeleteProductHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteProductHandlerTests"/> class.
    /// Sets up the test dependencies and creates fake data generators.
    /// </summary>
    public DeleteProductHandlerTests()
    {
        _productRepository = Substitute.For<IProductRepository>();
        _handler = new DeleteProductHandler(_productRepository);
    }
    
    /// <summary>
    /// Should throw a validation exception when the product ID is invalid.
    /// </summary>
    [Fact(DisplayName = "When product ID is empty Then should throw validation exception")]
    public async Task Handle_ShouldThrowValidationException_WhenProductIdIsEmpty()
    {
        // Arrange
        var command = new DeleteProductCommand(Guid.Empty);

        // Act
        var action = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await action.Should().ThrowAsync<ValidationException>();
    }
    
    /// <summary>
    /// Should throw a not found exception when the product does not exist in the database.
    /// </summary>
    [Fact(DisplayName = "When product does not exist Then should throw not found domain exception")]
    public async Task Handle_ShouldThrowNotFoundException_WhenProductIsMissing()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var command = new DeleteProductCommand(productId);

        // Act
        var action = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await action.Should()
                .ThrowAsync<KeyNotFoundException>()
                .WithMessage($"Product with ID {productId} not found");
    }
    
    /// <summary>
    /// Should return success result when a valid product is deleted.
    /// </summary>
    [Fact(DisplayName = "When deleting existing product Then should return success result")]
    public async Task Handle_ShouldReturnSuccess_WhenProductIsDeleted()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var command = new DeleteProductCommand(productId);

        _productRepository
            .DeleteAsync(productId, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(true));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();

        await _productRepository.Received(1)
            .DeleteAsync(productId, Arg.Any<CancellationToken>());
    }
}