using Ambev.DeveloperEvaluation.Application.Products;
using Ambev.DeveloperEvaluation.Application.Products.GetProduct;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Products;

/// <summary>
/// Contains unit tests for the <see cref="GetProductHandler"/> class.
/// </summary>
public class GetProductHandlerTests
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly GetProductHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetProductHandlerTests"/> class.
    /// Sets up the test dependencies and creates fake data generators.
    /// </summary>
    public GetProductHandlerTests()
    {
        _productRepository = Substitute.For<IProductRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new GetProductHandler(_productRepository, _mapper);
    }
    
    /// <summary>
    /// Should throw a validation exception when the product ID is empty.
    /// </summary>
    [Fact(DisplayName = "When product ID is empty Then should throw validation exception")]
    public async Task Handle_ShouldThrowValidationException_WhenProductIdIsEmpty()
    {
        // Given
        var command = new GetProductCommand(Guid.Empty);

        // When
        var method = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await method.Should().ThrowAsync<ValidationException>();
    }
    
    /// <summary>
    /// Should throw a not found exception when the product does not exist.
    /// </summary>
    [Fact(DisplayName = "When product is not found Then should throw key not found exception")]
    public async Task Handle_ShouldThrowKeyNotFoundException_WhenProductDoesNotExist()
    {
        // Given
        var productId = Guid.NewGuid();
        var command = new GetProductCommand(productId);

        // When
        var method = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await method.Should()
                .ThrowAsync<KeyNotFoundException>()
                .WithMessage($"Product with ID {productId} not found");
    }
    
    /// <summary>
    /// Should return product details when a valid product ID is provided.
    /// </summary>
    [Fact(DisplayName = "When getting product by valid ID Then should return product details")]
    public async Task Handle_ShouldReturnProductDetails_WhenProductExists()
    {
        // Given
        var product = ProductTestData.GenerateValid();
        var command = new GetProductCommand(product.Id);

        _mapper.Map<GetProductResult>(Arg.Any<Product>()).Returns(new GetProductResult
        {
            Id = product.Id,
            Image = product.Image,
            Price = product.Price,
            Category = product.Category,
            Description = product.Description,
            Title = product.Title,
            Rating = product.Rating,
        });
        _productRepository.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<Product?>(product));

        // When
        var queryResult = await _handler.Handle(command, CancellationToken.None);

        // Then
        queryResult.Should().NotBeNull();
        queryResult.Id.Should().Be(product.Id);
    }
}