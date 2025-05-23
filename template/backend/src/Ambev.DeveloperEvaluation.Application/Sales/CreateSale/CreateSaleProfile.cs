using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

/// <summary>
/// Profile for mapping between Sale entity and CreateSaleResponse
/// </summary>
public class CreateSaleProfile: Profile
{
    /// <summary>
    /// Initializes the mappings for CreateSale operation
    /// </summary>
    public CreateSaleProfile()
    {
        CreateMap<CreateSaleItemCommand, SaleItem>()
            .ConstructUsing(src => new SaleItem(
            src.ProductId,
            src.ProductName,
            src.Quantity,
            src.UnitPrice,
            0 // O desconto pode ser calculado depois se preferir
        ));
        CreateMap<SaleItem, CreateSaleItemResult>();
        CreateMap<CreateSaleCommand, Sale>()
            .ForMember(dest => dest.Items, opt => opt.MapFrom(s => s.Items));
        CreateMap<Sale, CreateSaleResult>()
            .ForMember(dest => dest.Items, opt => opt.MapFrom(s => s.Items));
    }
}