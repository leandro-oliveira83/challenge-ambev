using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetAllSale;

/// <summary>
/// Handler for processing GetAllSaleCommand requests
/// </summary>
public class GetAllSaleHandler : IRequestHandler<GetAllSaleCommand, GetAllSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of GetAllSaleHandler
    /// </summary>
    /// <param name="saleRepository">The sale repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    public GetAllSaleHandler(
        ISaleRepository saleRepository,
        IMapper mapper)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the GetAllSaleCommand request
    /// </summary>
    /// <param name="request">The GetAllProduct command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The user details if found</returns>
    public async Task<GetAllSaleResult> Handle(GetAllSaleCommand request, CancellationToken cancellationToken)
    {
        var allsales = await _saleRepository.GetAllPagedAsync(request, cancellationToken);

        return _mapper.Map<GetAllSaleResult>(allsales);
    }
    
}