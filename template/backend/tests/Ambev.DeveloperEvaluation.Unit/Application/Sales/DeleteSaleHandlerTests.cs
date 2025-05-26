using FluentAssertions;
using FluentValidation;
using NSubstitute;
using Xunit;
using Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Messaging;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales;

/// <summary>
/// Contains unit tests for the <see cref="DeleteSaleHandler"/> class.
/// </summary>
public class DeleteSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IEventPublisher _publisher;
    private readonly DeleteSaleHandler _handler;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteSaleHandlerTests"/> class.
    /// Sets up the test dependencies and creates fake data generators.
    /// </summary>
    public DeleteSaleHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _publisher = Substitute.For<IEventPublisher>();
        _handler = new DeleteSaleHandler(_saleRepository, _publisher);
    }
    
    [Fact(DisplayName = "Given invalid command Then should throw validation exception")]
    public async Task Handle_InvalidCommand_ShouldThrowValidationException()
    {
        // Arrange
        var command = new DeleteSaleCommand(Guid.Empty);

        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ValidationException>();
    }
    
    [Fact(DisplayName = "Given valid command but sale not found Then should throw not found exception")]
    public async Task Handle_SaleNotFound_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var command = new DeleteSaleCommand(Guid.NewGuid());
        _saleRepository.DeleteAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(false);

        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should()
            .ThrowAsync<KeyNotFoundException>()
            .WithMessage($"Sale with ID {command.Id} not found");
    }
    
    [Fact(DisplayName = "Given valid command Then should return success and publish event")]
    public async Task Handle_ValidCommand_ShouldReturnSuccessAndPublishEvent()
    {
        // Arrange
        var command = new DeleteSaleCommand(Guid.NewGuid());

        _saleRepository.DeleteAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();

        await _publisher.Received(1)
            .PublishAsync(Arg.Is<SaleDeletedEvent>(e => e.SaleId == command.Id), Arg.Any<CancellationToken>());
    }
}