using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;

/// <summary>
/// Profile for mapping between Application and API UpdateSale responses.
/// </summary>
public class UpdateSaleProfile: Profile
{
    /// <summary>
    /// Initializes the mappings for UpdateSale feature.
    /// </summary>
    public UpdateSaleProfile()
    {
        CreateMap<UpdateSaleItemRequest, UpdateSaleItemCommand>();
        CreateMap<UpdateSaleItemResult, UpdateSaleItemResponse>();
        
        CreateMap<UpdateSaleRequest, UpdateSaleCommand>()
            .ForMember(c => c.Items, opt => opt.MapFrom(s => s.Items));
        CreateMap<UpdateSaleResult, UpdateSaleResponse>()
            .ForMember(c => c.Items, opt => opt.MapFrom(s => s.Items));
        
    }
}