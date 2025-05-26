using Ambev.DeveloperEvaluation.Common.Results;
using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetAllSale;

/// <summary>
/// Profile for mapping between Sale entity and GetAllSaleResponse
/// </summary>
public class GetAllSaleProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for GetAllSale operation.
    /// </summary>
    public GetAllSaleProfile()
    {
        CreateMap<SaleItem, GetAllSaleItemResult>();
        CreateMap<Sale, SaleResult>();
        CreateMap<PaginationQueryResult<Sale>, GetAllSaleResult>();
    }
}