using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale;

/// <summary>
/// Profile for mapping between Sale entity and CreateSaleResponse
/// </summary>
public class GetSaleProfile: Profile
{
    /// <summary>
    /// Initializes the mappings for GetSale operation
    /// </summary>
    public GetSaleProfile()
    {
        CreateMap<SaleItem, GetSaleItemResult>();
        CreateMap<Sale, GetSaleResult>()
            .ForMember(dest => dest.Items, opt => opt.MapFrom(s => s.Items));
    }
}