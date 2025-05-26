using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.DeleteProduct;

/// <summary>
/// Profile for mapping DeleteUser feature requests to commands
/// </summary>
public class DeleteProductProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for DeleteSale feature
    /// </summary>
    public DeleteProductProfile()
    {
        CreateMap<Guid, Application.Products.DeleteProduct.DeleteProductCommand>()
            .ConstructUsing(id => new Application.Products.DeleteProduct.DeleteProductCommand(id));
    }
}