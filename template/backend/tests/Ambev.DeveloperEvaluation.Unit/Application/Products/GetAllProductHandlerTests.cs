using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Xunit;
using Ambev.DeveloperEvaluation.Application.Products.GetAllProduct;
using Ambev.DeveloperEvaluation.Common.Results;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
namespace Ambev.DeveloperEvaluation.Unit.Application.Products;

/// <summary>
/// Contains unit tests for the <see cref="GetAllProductHandler"/> class.
/// </summary>
public class GetAllProductHandlerTests
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly GetAllProductHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetAllProductHandlerTests"/> class.
    /// Sets up the test dependencies and creates fake data generators.
    /// </summary>
    public GetAllProductHandlerTests()
    {
        _productRepository = Substitute.For<IProductRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new GetAllProductHandler(_productRepository, _mapper);
    }
    
    /// <summary>
    /// Should return two products when the repository contains two items.
    /// </summary>
    [Fact(DisplayName = "When repository has two products Then handler should return result with two items")]
    public async Task Handle_ShouldReturnTwoProducts_WhenRepositoryHasTwoItems()
    {
        // Arrange
        var command = new GetAllProductCommand();

        ICollection<Product> products =
        [
            ProductTestData.GenerateValid(),
            ProductTestData.GenerateValid(),
        ];
        _mapper.Map<GetAllProductResult>(Arg.Any<PaginationQueryResult<Product>>()).Returns(new GetAllProductResult
        {
            Items = [new(), new()],
            TotalItems = products.Count,
        });
        _productRepository.GetAllPagedAsync(Arg.Any<PaginationQuery>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(new PaginationQueryResult<Product>
            {
                Items = products,
                TotalItems = products.Count,
            }));

        // Act
        var queryResult = await _handler.Handle(command, CancellationToken.None);

        // Assert
        queryResult.Should().NotBeNull();
        queryResult.Items.Should().HaveCount(2);
        queryResult.TotalItems.Should().Be(2);
    }
}