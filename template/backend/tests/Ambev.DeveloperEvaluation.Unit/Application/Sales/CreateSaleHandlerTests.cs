using AutoMapper;
using FluentAssertions;
using FluentValidation;
using NSubstitute;
using Xunit;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Messaging;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.Sales.TestData;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales;

/// <summary>
/// Contains unit tests for the <see cref="CreateSaleHandler"/> class.
/// </summary>
public class CreateSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly IEventPublisher _publisher;
    private readonly CreateSaleHandler _handler;

    public CreateSaleHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _productRepository = Substitute.For<IProductRepository>();
        _mapper = Substitute.For<IMapper>();
        _publisher = Substitute.For<IEventPublisher>();
        _handler = new CreateSaleHandler(_saleRepository, _productRepository, _mapper, _publisher);
    }

    [Fact(DisplayName = "Given invalid command Then validation should fail")]
    public async Task Handle_InvalidCommand_ShouldThrowValidationException()
    {
        // Arrange
        var command = new CreateSaleCommand();
        
        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);
        
        // Assert
        await act.Should().ThrowAsync<ValidationException>();
    }

    [Fact(DisplayName = "Given a non-existing product Then should throw validation error")]
    public async Task Handle_NonExistingProduct_ShouldThrowValidationException()
    {
        // Arrange
        var command = CreateSaleHandlerTestData.GenerateValidCommand();
        _productRepository.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns((Product?)null);

        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);
        
        // Assert
        await act.Should().ThrowAsync<ValidationException>()
            .WithMessage("*does not exist*");
    }

    [Fact(DisplayName = "Given product with invalid quantity Then should throw validation error")]
    public async Task Handle_TooManyItems_ShouldThrowValidationException()
    {
        // Arrange
        var command = CreateSaleHandlerTestData.GenerateValidCommandWithQuantity(30);

        _productRepository.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns(ProductTestData.GenerateValid());

        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ValidationException>()
            .WithMessage("*more than 20 identical items*");
    }

    [Fact(DisplayName = "Given valid sale data Then should return success and publish event")]
    public async Task Handle_ValidRequest_ShouldReturnSuccessAndPublishEvent()
    {
        // Arrange
        var command = CreateSaleHandlerTestData.GenerateValidCommand();
        var createdSale = new Sale("SN123", DateTime.UtcNow, "cust123", "John Doe", "br001", "Main Branch");

        _productRepository.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns(ProductTestData.GenerateValid());

        _saleRepository.CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
            .Returns(createdSale);

        _mapper.Map<CreateSaleResult>(Arg.Any<Sale>()).Returns(new CreateSaleResult
        {
            Id = Guid.NewGuid(),
            SaleNumber = "SN123",
            BranchName = "Main Branch",
            CustomerName = "John Doe"
        });

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.SaleNumber.Should().Be("SN123");
        await _publisher.Received(1).PublishAsync(Arg.Any<SaleCreatedEvent>(), Arg.Any<CancellationToken>());
    }
    
    [Fact(DisplayName = "Given quantity less than 4 Then no discount should be applied")]
    public async Task Handle_QuantityLessThan4_ShouldApplyNoDiscount()
    {
        // Arrange
        var command = CreateSaleHandlerTestData.GenerateValidCommandWithQuantity(3);

        var capturedSale = new Sale();
        _productRepository.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns(ProductTestData.GenerateValid());
    
        _saleRepository.CreateAsync(Arg.Do<Sale>(sale =>
        {
            capturedSale = sale;
        }), Arg.Any<CancellationToken>()).Returns(capturedSale);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        var item = capturedSale.Items.First();
        item.Discount.Should().Be(0m);
    }
    
    [Fact(DisplayName = "Given quantity between 4 and 9 Then 10 percent discount should be applied")]
    public async Task Handle_QuantityBetween4And9_ShouldApply10PercentDiscount()
    {
        // Arrange
        var command = CreateSaleHandlerTestData.GenerateValidCommandWithQuantity(6);

        var capturedSale = new Sale();
        _productRepository.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns(ProductTestData.GenerateValid());
    
        _saleRepository.CreateAsync(Arg.Do<Sale>(sale =>
        {
            capturedSale = sale;
        }), Arg.Any<CancellationToken>()).Returns(capturedSale);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        var item = capturedSale.Items.First();
        item.Discount.Should().Be(0.10m);
    }
    
    [Fact(DisplayName = "Given quantity between 10 and 20 Then 20 percent discount should be applied")]
    public async Task Handle_QuantityBetween10And20_ShouldApply20PercentDiscount()
    {
        // Arrange
        var command = CreateSaleHandlerTestData.GenerateValidCommandWithQuantity(15);

        var capturedSale = new Sale();
        _productRepository.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns(ProductTestData.GenerateValid());

        _saleRepository.CreateAsync(Arg.Do<Sale>(sale =>
        {
            capturedSale = sale;
        }), Arg.Any<CancellationToken>()).Returns(capturedSale);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        var item = capturedSale.Items.First();
        item.Discount.Should().Be(0.20m);
    }
    
    [Fact(DisplayName = "When creating sale Then total amount should be calculated correctly")]
    public async Task Handle_ShouldCalculateTotalCorrectly()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var command = new CreateSaleCommand
        {
            SaleNumber = "SALE123",
            Date = DateTime.UtcNow,
            CustomerId = "cust-1",
            CustomerName = "Test",
            BranchId = "br-1",
            BranchName = "Branch",
            Items = new List<CreateSaleItemCommand>
            {
                new()
                {
                    ProductId = productId,
                    ProductName = "Product A",
                    Quantity = 10, // 20% discount
                    UnitPrice = 100
                }
            }
        };

        var capturedSale = new Sale();
        _productRepository.GetByIdAsync(productId, Arg.Any<CancellationToken>())
            .Returns(ProductTestData.GenerateValid());

        _saleRepository.CreateAsync(Arg.Do<Sale>(sale =>
        {
            capturedSale = sale;
        }), Arg.Any<CancellationToken>()).Returns(capturedSale);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        var expectedTotal = 10 * 100 * 0.8m;
        capturedSale.TotalAmount.Should().Be(expectedTotal);
    }
}