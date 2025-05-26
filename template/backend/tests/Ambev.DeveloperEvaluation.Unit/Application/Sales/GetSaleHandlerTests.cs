using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales;

/// <summary>
/// Contains unit tests for the <see cref="GetSaleHandler"/> class.
/// </summary>
public class GetSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly GetSaleHandler _handler;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="GetSaleHandlerTests"/> class.
    /// Sets up the test dependencies and creates fake data generators.
    /// </summary>
    public GetSaleHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new GetSaleHandler(_saleRepository, _mapper);
    }

    /// <summary>
    /// Ensures that a validation exception is thrown when the command has an invalid ID.
    /// </summary>
    [Fact(DisplayName = "Given invalid command Then should throw validation exception")]
    public async Task Handle_InvalidCommand_ShouldThrowValidationException()
    {
        // Arrange
        var command = new GetSaleCommand(Guid.Empty);

        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ValidationException>();
    }

    /// <summary>
    /// Ensures that a key not found exception is thrown when the sale does not exist in the repository.
    /// </summary>
    [Fact(DisplayName = "Given non-existing sale Then should throw not found exception")]
    public async Task Handle_SaleNotFound_ShouldThrowNotFoundException()
    {
        // Arrange
        var id = Guid.NewGuid();
        var command = new GetSaleCommand(id);
        _saleRepository.GetByIdAsync(id, Arg.Any<CancellationToken>())
            .Returns((Sale)null);

        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"Sale with ID {id} not found");
    }

    /// <summary>
    /// Verifies that a valid request returns the expected mapped sale result.
    /// </summary>
    [Fact(DisplayName = "Given valid command Then should return sale result")]
    public async Task Handle_ValidCommand_ShouldReturnMappedResult()
    {
        // Arrange
        var id = Guid.NewGuid();
        var command = new GetSaleCommand(id);
        var sale = new Sale("SN123", DateTime.UtcNow, "CUST1", "Customer", "BR1", "Branch");

        _saleRepository.GetByIdAsync(id, Arg.Any<CancellationToken>())
            .Returns(sale);

        _mapper.Map<GetSaleResult>(sale).Returns(new GetSaleResult
        {
            Id = id,
            SaleNumber = "SN123",
            CustomerName = "Customer",
            BranchName = "Branch",
            TotalAmount = 0
        });

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.SaleNumber.Should().Be("SN123");
    }
}