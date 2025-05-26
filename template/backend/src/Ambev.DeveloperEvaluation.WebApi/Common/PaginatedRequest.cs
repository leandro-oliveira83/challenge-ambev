using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using FluentValidation;
using Ambev.DeveloperEvaluation.Common.Enums;

namespace Ambev.DeveloperEvaluation.WebApi.Common;

/// <summary>
/// Represents pagination and sorting input from query string.
/// </summary>
public class PaginatedRequest
{
    private const int DefaultPage = 1;
    private const int DefaultSize = 10;

    [FromQuery(Name = "_page")]
    public int? Page { get; init; }

    [FromQuery(Name = "_size")]
    public int? PageSize { get; init; }

    [FromQuery(Name = "_order")]
    public string? Sort { get; init; }

    /// <summary>
    /// Gets the parsed sort instructions.
    /// </summary>
    [BindNever]
    public IEnumerable<KeyValuePair<string, SortDirection>> SortInstructions => ParseSort(Sort);

    public int CurrentPage => Page.GetValueOrDefault(DefaultPage);

    public int CurrentSize => PageSize.GetValueOrDefault(DefaultSize);

    private static IEnumerable<KeyValuePair<string, SortDirection>> ParseSort(string? sort)
    {
        if (string.IsNullOrWhiteSpace(sort))
            return Enumerable.Empty<KeyValuePair<string, SortDirection>>();

        return sort
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(item =>
            {
                var tokens = item.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                var field = tokens[0];
                var direction = tokens.Length > 1
                    ? ParseDirection(tokens[1])
                    : SortDirection.Asc;

                return new KeyValuePair<string, SortDirection>(field, direction);
            });
    }

    private static SortDirection ParseDirection(string raw)
    {
        return raw.ToLower() switch
        {
            "asc" => SortDirection.Asc,
            "desc" => SortDirection.Desc,
            _ => throw new ValidationException($"Invalid sort direction '{raw}'. Use 'asc' or 'desc'.")
        };
    }
}