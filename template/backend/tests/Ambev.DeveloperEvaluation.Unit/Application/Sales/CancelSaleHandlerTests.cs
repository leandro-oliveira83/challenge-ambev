using FluentAssertions;
using FluentValidation;
using NSubstitute;
using Xunit;
using Ambev.DeveloperEvaluation.Application.Sales.CancelSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Messaging;
using Ambev.DeveloperEvaluation.Domain.Repositories;


namespace Ambev.DeveloperEvaluation.Unit.Application.Sales;

/// <summary>
/// Contains unit tests for the <see cref="CancelSaleHandler"/> class.
/// </summary>
public class CancelSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IEventPublisher _publisher;
    private readonly CancelSaleHandler _handler;

    public CancelSaleHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _publisher = Substitute.For<IEventPublisher>();
        _handler = new CancelSaleHandler(_saleRepository, _publisher);
    }

    /// <summary>
    /// Ensures that a validation exception is thrown when an invalid CancelSaleCommand is provided.
    /// </summary>
    [Fact(DisplayName = "Given invalid cancel command Then should throw validation exception")]
    public async Task Handle_InvalidCommand_ShouldThrowValidationException()
    {
        // Arrange
        var command = new CancelSaleCommand(Guid.Empty);

        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ValidationException>();
    }
    
    /// <summary>
    /// Ensures that a KeyNotFoundException is thrown when the specified sale ID does not exist.
    /// </summary>
    [Fact(DisplayName = "Given nonexistent sale ID Then should throw key not found exception")]
    public async Task Handle_SaleNotFound_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var id = Guid.NewGuid();
        var command = new CancelSaleCommand(id);
        _saleRepository.GetByIdAsync(id, Arg.Any<CancellationToken>()).Returns((Sale?)null);

        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"Sale with ID {id} not found");
    }
    
    /// <summary>
    /// Verifies that a valid cancel request properly cancels the sale and publishes a SaleCancelledEvent.
    /// </summary>
    [Fact(DisplayName = "Given valid cancel request Then should cancel sale and publish event")]
    public async Task Handle_ValidRequest_ShouldCancelSaleAndPublishEvent()
    {
        // Arrange
        var id = Guid.NewGuid();
        var command = new CancelSaleCommand(id);
        var sale = new Sale("SALE123", DateTime.UtcNow, "cust-1", "Customer", "br-1", "Branch");

        _saleRepository.GetByIdAsync(id, Arg.Any<CancellationToken>()).Returns(sale);
        _saleRepository.UpdateAsync(sale, Arg.Any<CancellationToken>()).Returns(sale);

        // Act
        var response = await _handler.Handle(command, CancellationToken.None);

        // Assert
        response.Success.Should().BeTrue();
        sale.IsCancelled.Should().BeTrue();
        await _publisher.Received(1).PublishAsync(Arg.Any<SaleCancelledEvent>(), Arg.Any<CancellationToken>());
    }
}