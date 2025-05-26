using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Sales;
using Ambev.DeveloperEvaluation.Application.Sales.GetAllSale;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetAllSale;

public class GetAllSaleProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for GetAllSale feature.
    /// </summary>
    public GetAllSaleProfile()
    {
        CreateMap<GetAllSaleRequest, GetAllSaleCommand>();
        CreateMap<GetAllSaleItemResult, GetAllSaleItemResponse>();
        CreateMap<SaleResult, GetAllSaleResponse>();
    }
}