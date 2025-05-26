using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

/// <summary>
/// Profile for mapping between Application and API CreateSale responses.
/// </summary>
public class CreateSaleProfile: Profile
{
    /// <summary>
    /// Initializes the mappings for CreateSale feature.
    /// </summary>
    public CreateSaleProfile()
    {
        CreateMap<CreateSaleItemRequest, CreateSaleItemCommand>();
        CreateMap<CreateSaleItemResult, CreateSaleItemResponse>();
        
        CreateMap<CreateSaleRequest, CreateSaleCommand>()
            .ForMember(c => c.Items, opt => opt.MapFrom(s => s.Items));
        CreateMap<CreateSaleResult, CreateSaleResponse>()
            .ForMember(c => c.Items, opt => opt.MapFrom(s => s.Items));
        
    }
}