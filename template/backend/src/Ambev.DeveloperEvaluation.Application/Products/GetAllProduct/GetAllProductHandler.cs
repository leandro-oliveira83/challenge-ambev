using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.GetAllProduct;

/// <summary>
/// Handler for processing GetAllProductCommand requests
/// </summary>
public class GetAllProductHandler : IRequestHandler<GetAllProductCommand, GetAllProductResult>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of GetAllProductHandler
    /// </summary>
    /// <param name="productRepository">The product repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    public GetAllProductHandler(
        IProductRepository productRepository,
        IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the GetAllProductCommand request
    /// </summary>
    /// <param name="request">The GetAllProduct command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The user details if found</returns>
    public async Task<GetAllProductResult> Handle(GetAllProductCommand request, CancellationToken cancellationToken)
    {
        var allProducts = await _productRepository.GetAllPagedAsync(request, cancellationToken);

        return _mapper.Map<GetAllProductResult>(allProducts);
    }
    
}