using MediatR;
using Ambev.DeveloperEvaluation.Common.Results;

namespace Ambev.DeveloperEvaluation.Application.Products.GetAllProduct;

/// <summary>
/// Command for retrieving all product list.
/// </summary>
public class GetAllProductCommand : PaginationQuery, IRequest<GetAllProductResult>
{
}