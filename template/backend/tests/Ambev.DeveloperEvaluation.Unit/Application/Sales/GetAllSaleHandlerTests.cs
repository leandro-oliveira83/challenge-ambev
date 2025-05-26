using FluentAssertions;
using NSubstitute;
using Xunit;
using Ambev.DeveloperEvaluation.Application.Sales.GetAllSale;
using Ambev.DeveloperEvaluation.Common.Results;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales;

/// <summary>
/// Contains unit tests for the <see cref="GetAllSaleHandler"/> class.
/// </summary>
public class GetAllSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly GetAllSaleHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetAllSaleHandlerTests"/> class.
    /// Sets up the test dependencies and creates fake data generators.
    /// </summary>
    public GetAllSaleHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new GetAllSaleHandler(_saleRepository, _mapper);
    }
    
    [Fact(DisplayName = "Given valid request When sales exist Then should return paginated result")]
    public async Task Handle_WithValidRequest_ShouldReturnPaginatedSales()
    {
        // Arrange
        var command = new GetAllSaleCommand { Page = 1, Size = 10 };
        var sales = new PaginationQueryResult<Sale>
        {
            Items = new List<Sale>
            {
                new("SN001", DateTime.UtcNow, "CUST01", "John Doe", "BR001", "Branch A"),
                new("SN002", DateTime.UtcNow, "CUST02", "Jane Doe", "BR002", "Branch B"),
            },
            TotalItems = 2
        };

        _saleRepository.GetAllPagedAsync(Arg.Any<PaginationQuery>(), Arg.Any<CancellationToken>())
            .Returns(sales);

        _mapper.Map<GetAllSaleResult>(sales)
            .Returns(new GetAllSaleResult
            {
                Items = new List<SaleResult>
                {
                    new() { SaleNumber = "SN001", CustomerName = "John Doe" },
                    new() { SaleNumber = "SN002", CustomerName = "Jane Doe" }
                },
                TotalItems = 2
            });

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(2);
        result.TotalItems.Should().Be(2);
    }
    
    [Fact(DisplayName = "Given empty result When no sales found Then should return empty result")]
    public async Task Handle_WhenNoSales_ShouldReturnEmpty()
    {
        // Arrange
        var command = new GetAllSaleCommand { Page = 1, Size = 10 };
        var emptySales = new PaginationQueryResult<Sale>
        {
            Items = [],
            TotalItems = 0
        };

        _saleRepository.GetAllPagedAsync(Arg.Any<PaginationQuery>(), Arg.Any<CancellationToken>())
            .Returns(emptySales);

        _mapper.Map<GetAllSaleResult>(emptySales)
            .Returns(new GetAllSaleResult
            {
                Items = [],
                TotalItems = 0
            });

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().BeEmpty();
        result.TotalItems.Should().Be(0);
    }
}