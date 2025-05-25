using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Products;
using Ambev.DeveloperEvaluation.Application.Products.GetAllProduct;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.GetAllProduct;

public class GetAllProductProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for GetAllProduct feature.
    /// </summary>
    public GetAllProductProfile()
    {
        CreateMap<GetAllProductRequest, GetAllProductCommand>();
        CreateMap<ProductResult, GetAllProductResponse>();
    }
}