namespace Ambev.DeveloperEvaluation.Common.Results;

/// <summary>
/// Represents the result from pagination.
/// </summary>
/// <typeparam name="T"></typeparam>
public class PaginationQueryResult<T>
{
    /// <summary>
    /// Items of pagination.
    /// </summary>
    public ICollection<T> Items { get; init; } = [];
    
    /// <summary>
    /// Total items of pagination.
    /// </summary>
    public int TotalItems { get; init; }
    
    /// <summary>
    /// Current page.
    /// </summary>
    public int CurrentPage { get; init; }
    
    /// <summary>
    /// Total of pages.
    /// </summary>
    public int TotalPages { get; init; }
    
}