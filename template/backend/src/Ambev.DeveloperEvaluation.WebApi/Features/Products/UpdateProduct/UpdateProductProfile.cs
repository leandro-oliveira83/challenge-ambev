using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.UpdateProduct;

/// <summary>
/// Profile for mapping between Application and API UpdateProduct responses.
/// </summary>
public class UpdateProductProfile: Profile
{
    /// <summary>
    /// Initializes the mappings for UpdateProduct feature.
    /// </summary>
    public UpdateProductProfile()
    {
        CreateMap<UpdateProductRequest, UpdateProductCommand>();
        CreateMap<UpdateProductResult, UpdateProductResponse>();
    }
}