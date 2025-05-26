using Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.Products.TestData;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Products;

/// <summary>
/// Contains unit tests for the <see cref="UpdateProductHandler"/> class.
/// </summary>
public class UpdateProductHandlerTests
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly UpdateProductHandler _handler;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateProductHandlerTests"/> class.
    /// Sets up the test dependencies and creates fake data generators.
    /// </summary>
    public UpdateProductHandlerTests()
    {
        _productRepository = Substitute.For<IProductRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new UpdateProductHandler(_productRepository, _mapper);
    }
    
    /// <summary>
    /// Should return validation errors when the update product command is invalid.
    /// </summary>
    [Fact(DisplayName = "When validating an invalid update product command Then should return validation errors")]
    public void Validate_ShouldReturnErrors_WhenUpdateCommandIsInvalid()
    {
        // Arrange
        var command = new UpdateProductCommand();

        // Act
        var validationResult = command.Validate();

        // Assert
        validationResult.Should().NotBeNull();
        validationResult.Errors.Should().NotBeEmpty();
    }
    
    /// <summary>
    /// Should throw a validation exception when the update product command is invalid.
    /// </summary>
    [Fact(DisplayName = "When handling an invalid update product command Then should throw validation exception")]
    public async Task Handle_ShouldThrowValidationException_WhenUpdateCommandIsInvalid()
    {
        // Arrange
        var command = new UpdateProductCommand();

        // Act
        var method = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await method.Should().ThrowAsync<ValidationException>();
    }
    
    /// <summary>
    /// Should throw a not found exception when trying to update a product that does not exist.
    /// </summary>
    [Fact(DisplayName = "When updating a non-existent product Then should throw key not found exception")]
    public async Task Handle_ShouldThrowKeyNotFoundException_WhenProductDoesNotExist()
    {
        // Arrange
        var command = UpdateProductHandlerTestData.GenerateValidCommand();
        _productRepository.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<Product?>(null));

        // Act
        var method = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await method.Should()
                .ThrowAsync<KeyNotFoundException>()
            .WithMessage($"Product with ID {command.Id} not found");
    }
    
    
    /// <summary>
    /// Should throw a domain exception when updating a product with a title that already exists on another product.
    /// </summary>
    [Fact(DisplayName = "When updating product with duplicate title Then should throw domain exception")]
    public async Task Handle_ShouldThrowDomainException_WhenProductTitleAlreadyExists()
    {
        // Arrange
        var command = UpdateProductHandlerTestData.GenerateValidCommand();
        var product = UpdateProductHandlerTestData.CreateProductByCommand(command);

        _productRepository.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<Product?>(product));

        product.Id = Guid.NewGuid(); // note: to simulate another product with same title.
        _productRepository.GetByTitleAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<Product?>(product));
        
        // Act
        var method = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await method.Should()
            .ThrowAsync<InvalidOperationException>()
            .WithMessage($"Product with title {command.Title} already exists");
    }
    
    /// <summary>
    /// Should return a successful result when updating an existing product with valid data.
    /// </summary>
    [Fact(DisplayName = "When updating a valid existing product Then should return updated product result")]
    public async Task Handle_ShouldReturnSuccessResult_WhenUpdatingValidProduct()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var command = UpdateProductHandlerTestData.GenerateValidCommand();
        var product = UpdateProductHandlerTestData.CreateProductByCommand(command);

        _productRepository.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<Product?>(product));

        _mapper.Map<Product>(Arg.Any<UpdateProductCommand>())
            .Returns(product);
        _mapper.Map<UpdateProductResult>(Arg.Any<Product>())
            .Returns(new UpdateProductResult
            {
                Id = productId,
                Title = command.Title,
                Description = command.Description,
                Category = command.Category,
                Price = command.Price,
                Image = command.Image,
                Rating = command.Rating,
            });

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(productId);
    }
    
    /// <summary>
    /// Should return a successful result when updating a valid product without checking for category existence.
    /// </summary>
    [Fact(DisplayName = "When updating a valid product without category validation Then should return success result")]
    public async Task Handle_ShouldReturnSuccessResult_WhenUpdatingProductWithoutCategoryCheck()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var command = UpdateProductHandlerTestData.GenerateValidCommand();
        var product = UpdateProductHandlerTestData.CreateProductByCommand(command);

        _mapper.Map<Product>(Arg.Any<UpdateProductCommand>())
            .Returns(product);
        _mapper.Map<UpdateProductResult>(Arg.Any<Product>())
            .Returns(new UpdateProductResult
            {
                Id = productId,
                Title = command.Title,
                Description = command.Description,
                Category = command.Category,
                Price = command.Price,
                Image = command.Image,
                Rating = command.Rating,
            });

        _productRepository.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<Product?>(product));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(productId);
    }
}