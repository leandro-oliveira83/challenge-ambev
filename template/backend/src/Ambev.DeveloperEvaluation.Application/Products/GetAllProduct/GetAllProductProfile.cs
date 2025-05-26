using Ambev.DeveloperEvaluation.Common.Results;
using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Products.GetAllProduct;

/// <summary>
/// Profile for mapping between Product entity and GetAllProductResponse
/// </summary>
public class GetAllProductProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for GetAllProduct operation.
    /// </summary>
    public GetAllProductProfile()
    {
        CreateMap<PaginationQueryResult<Product>, GetAllProductResult>();
        CreateMap<Product, ProductResult>();
    }
}