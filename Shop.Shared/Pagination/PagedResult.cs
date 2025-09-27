namespace Shop.Shared.Pagination;

/// <summary>
/// Represents a page of results with pagination metadata
/// </summary>
/// <typeparam name="T">The type of items in the page</typeparam>
public class PagedResult<T>
{
    public List<T> Items { get; init; } = new();
    public int TotalCount { get; init; }
    public int Page { get; init; }
    public int PageSize { get; init; }
    public int TotalPages => PageSize > 0 ? (int)Math.Ceiling((double)TotalCount / PageSize) : 0;
    public bool HasNextPage => Page < TotalPages;
    public bool HasPreviousPage => Page > 1;
    public int StartIndex => (Page - 1) * PageSize + 1;
    public int EndIndex => Math.Min(Page * PageSize, TotalCount);

    /// <summary>
    /// Creates a new paged result
    /// </summary>
    public static PagedResult<T> Create(List<T> items, int totalCount, int page, int pageSize)
    {
        return new PagedResult<T>
        {
            Items = items ?? new List<T>(),
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }

    /// <summary>
    /// Creates an empty paged result
    /// </summary>
    public static PagedResult<T> Empty(int page = 1, int pageSize = 10)
    {
        return new PagedResult<T>
        {
            Items = new List<T>(),
            TotalCount = 0,
            Page = page,
            PageSize = pageSize
        };
    }
}