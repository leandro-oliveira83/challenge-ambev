using System.Linq.Expressions;
using Ambev.DeveloperEvaluation.Common.Enums;
using Ambev.DeveloperEvaluation.Common.Results;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.Common.Extensions;

public static class PaginationExtensions
{
    public static async Task<PaginationQueryResult<T>> ApplyPaginationAsync<T>(
        this IQueryable<T> source,
        int page,
        int pageSize,
        Expression<Func<T, object>>? orderBy = null,
        SortDirection direction = SortDirection.Asc,
        CancellationToken cancellationToken = default)
        where T : class
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (page <= 0) page = 1;
        if (pageSize <= 0) pageSize = 10;

        var totalItems = await source.CountAsync(cancellationToken);

        if (orderBy != null)
        {
            source = direction == SortDirection.Asc
                ? source.OrderBy(orderBy)
                : source.OrderByDescending(orderBy);
        }

        var items = await source
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PaginationQueryResult<T>
        {
            Items = items,
            TotalItems = totalItems,
            CurrentPage = page,
            TotalPages = (int)Math.Ceiling(totalItems / (double)Math.Max(pageSize, 1))
        };
    }
}