using MediatR;
using Ambev.DeveloperEvaluation.Common.Results;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetAllSale;

/// <summary>
/// Command for retrieving all sale list.
/// </summary>
public class GetAllSaleCommand : PaginationQuery, IRequest<GetAllSaleResult>
{
    
}