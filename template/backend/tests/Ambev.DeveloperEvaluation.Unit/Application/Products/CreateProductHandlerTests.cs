using AutoMapper;
using FluentAssertions;
using FluentValidation;
using NSubstitute;
using Xunit;
using Ambev.DeveloperEvaluation.Application.Products.CreateProduct;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.Products.TestData;

namespace Ambev.DeveloperEvaluation.Unit.Application.Products;

/// <summary>
/// Contains unit tests for the <see cref="CreateProductHandler"/> class.
/// </summary>
public class CreateProductHandlerTests
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly CreateProductHandler _handler;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateProductHandlerTests"/> class.
    /// Sets up the test dependencies and creates fake data generators.
    /// </summary>
    public CreateProductHandlerTests()
    {
        _productRepository = Substitute.For<IProductRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new CreateProductHandler(_productRepository, _mapper);
    }
    
    /// <summary>
    /// Tests that an invalid Then validation should fail.
    /// </summary>
    [Fact(DisplayName = "Given invalid command Then validation should fail")]
    public void Handle_InvalidRequest_ReturnValidationError()
    {
        // Given
        var command = new CreateProductCommand();

        // When
        var validationResult = command.Validate();

        // Then
        validationResult.Should().NotBeNull();
        validationResult.Errors.Should().NotBeEmpty();
    }
    
    /// <summary>
    /// Should throw a validation exception when attempting to handle an invalid create product command.
    /// </summary>
    [Fact(DisplayName = "When handling invalid create command Then should throw validation exception")]
    public async Task Handle_ShouldThrowValidationException_WhenCommandIsInvalid()
    {
        // Given
        var command = new CreateProductCommand();

        // When
        var method = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await method.Should().ThrowAsync<ValidationException>();
    }
    
    /// <summary>
    /// Should throw a domain exception when creating a product with an existing title.
    /// </summary>
    [Fact(DisplayName = "When creating a product with existing title Then should throw domain exception")]
    public async Task Handle_ShouldThrowDomainException_WhenTitleAlreadyExists()
    {
        // Given
        var command = CreateProductHandlerTestData.GenerateValidCommand();
        _productRepository.GetByTitleAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<Product?>(new Product()));

        // When
        var method = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await method.Should()
            .ThrowAsync<InvalidOperationException>()
            .WithMessage($"Product with title {command.Title} already exists");
    }
    
    /// <summary>
    /// Should return a success response when handling a valid create product command.
    /// </summary>
    [Fact(DisplayName = "When handling valid create product command Then should return success result")]
    public async Task Handle_ShouldReturnSuccessResult_WhenCommandIsValid()
    {
        // Given
        var productId = Guid.NewGuid();
        var command = CreateProductHandlerTestData.GenerateValidCommand();
        
        _mapper.Map<Product>(Arg.Any<CreateProductCommand>());
        _mapper.Map<CreateProductResult>(Arg.Any<Product>())
            .Returns(new CreateProductResult
            {
                Id = productId,
                Description = command.Description,
                Category = command.Category,
                Image = command.Image,
                Price = command.Price,
                Rating = command.Rating,
                Title = command.Title,
            });

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.Id.Should().Be(productId);
    }
    
}