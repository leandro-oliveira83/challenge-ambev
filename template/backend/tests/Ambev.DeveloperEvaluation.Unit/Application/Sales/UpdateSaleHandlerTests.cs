using AutoMapper;
using FluentAssertions;
using FluentValidation;
using NSubstitute;
using Xunit;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Messaging;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.Sales.TestData;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales;

/// <summary>
/// Contains unit tests for the <see cref="UpdateSaleHandler"/> class.
/// </summary>
public class UpdateSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly IEventPublisher _publisher;
    private readonly UpdateSaleHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateSaleHandlerTests"/> class.
    /// Sets up the test dependencies and creates fake data generators.
    /// </summary>
    public UpdateSaleHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _productRepository = Substitute.For<IProductRepository>();
        _mapper = Substitute.For<IMapper>();
        _publisher = Substitute.For<IEventPublisher>();
        _handler = new UpdateSaleHandler(_saleRepository, _productRepository, _mapper, _publisher);
    }
    
    [Fact(DisplayName = "Given invalid command Then should throw validation error")]
    public async Task Handle_InvalidCommand_ShouldThrowValidation()
    {
        var command = new UpdateSaleCommand();
        var act = () => _handler.Handle(command, CancellationToken.None);
        await act.Should().ThrowAsync<ValidationException>();
    }
    
    [Fact(DisplayName = "Given non-existing sale Then should throw not found")]
    public async Task Handle_SaleNotFound_ShouldThrow()
    {
        var command = UpdateSaleHandlerTestData.GenerateValidCommand();
        _saleRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>()).Returns((Sale?)null);

        var act = () => _handler.Handle(command, CancellationToken.None);
        await act.Should().ThrowAsync<KeyNotFoundException>();
    }
    
    [Fact(DisplayName = "Given item with non-existing product Then should throw validation error")]
    public async Task Handle_ProductNotFound_ShouldThrow()
    {
        var command = UpdateSaleHandlerTestData.GenerateValidCommand();
        _saleRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(UpdateSaleHandlerTestData.CreateSaleByCommand(command));

        _productRepository.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns((Product?)null);

        var act = () => _handler.Handle(command, CancellationToken.None);
        await act.Should().ThrowAsync<ValidationException>();
    }
    
    [Fact(DisplayName = "Given valid update Then should update sale and publish events")]
    public async Task Handle_ValidUpdate_ShouldUpdateSaleAndPublishEvents()
    {
        var command = UpdateSaleHandlerTestData.GenerateValidCommand();
        var existingSale = UpdateSaleHandlerTestData.CreateSaleByCommand(command);

        _saleRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>()).Returns(existingSale);
        _productRepository.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns(ProductTestData.GenerateValid());

        _saleRepository.UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
            .Returns(existingSale);

        _mapper.Map<UpdateSaleResult>(Arg.Any<Sale>()).Returns(new UpdateSaleResult
        {
            Id = existingSale.Id,
            SaleNumber = existingSale.SaleNumber
        });

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().NotBeNull();
        result.SaleNumber.Should().Be(existingSale.SaleNumber);
        await _publisher.Received(1).PublishAsync(Arg.Any<SaleModifiedEvent>(), Arg.Any<CancellationToken>());
    }
    
    [Fact(DisplayName = "Given removed items in update Then should mark them as cancelled")]
    public async Task Handle_RemovedItems_ShouldMarkAsCancelledAndPublish()
    {
        var command = UpdateSaleHandlerTestData.GenerateValidCommand();

        var originalItem = new SaleItem(
            productId: command.Items[0].ProductId,
            productName: "Old Product",
            quantity: 5,
            unitPrice: 50,
            discount: 0.1m
        );

        var sale = new Sale("SALE123", DateTime.UtcNow, "cust", "Customer", "branch", "Branch");
        sale.Items.Add(originalItem);

        _saleRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>()).Returns(sale);
        _productRepository.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns(ProductTestData.GenerateValid());

        _saleRepository.UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
            .Returns(sale);

        _mapper.Map<UpdateSaleResult>(Arg.Any<Sale>()).Returns(new UpdateSaleResult { Id = sale.Id });

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().NotBeNull();
        originalItem.IsCancelled.Should().BeTrue();
    }
}