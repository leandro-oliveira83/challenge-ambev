using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

/// <summary>
/// Profile for mapping between Sale entity and CreateSaleResponse
/// </summary>
public class UpdateSaleProfile: Profile
{
    /// <summary>
    /// Initializes the mappings for CreateSale operation
    /// </summary>
    public UpdateSaleProfile()
    {
        CreateMap<UpdateSaleItemCommand, SaleItem>()
            .ConstructUsing(src => new SaleItem(
                src.ProductId,
                src.ProductName,
                src.Quantity,
                src.UnitPrice,
                0 
            ));
        CreateMap<SaleItem, UpdateSaleItemResult>();
        CreateMap<UpdateSaleCommand, Sale>()
            .ForMember(dest => dest.Items, opt => opt.MapFrom(s => s.Items));
        CreateMap<Sale, UpdateSaleResult>()
            .ForMember(dest => dest.Items, opt => opt.MapFrom(s => s.Items));
    }
}