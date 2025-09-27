namespace Shop.Shared.Pagination;

/// <summary>
/// Request object for pagination parameters
/// </summary>
public class PaginationRequest
{
    private int _page = 1;
    private int _pageSize = 10;

    /// <summary>
    /// Page number (1-based)
    /// </summary>
    public int Page 
    { 
        get => _page; 
        set => _page = value < 1 ? 1 : value; 
    }

    /// <summary>
    /// Number of items per page (1-100)
    /// </summary>
    public int PageSize 
    { 
        get => _pageSize; 
        set => _pageSize = value switch
        {
            < 1 => 10,
            > 100 => 100,
            _ => value
        };
    }

    /// <summary>
    /// Calculate the number of items to skip for pagination
    /// </summary>
    public int Skip => (Page - 1) * PageSize;

    /// <summary>
    /// Get the take amount for pagination (same as PageSize)
    /// </summary>
    public int Take => PageSize;

    /// <summary>
    /// Validates and corrects pagination values
    /// </summary>
    public void Validate()
    {
        if (Page < 1) Page = 1;
        if (PageSize < 1) PageSize = 10;
        if (PageSize > 100) PageSize = 100;
    }

    /// <summary>
    /// Creates a default pagination request (page 1, size 10)
    /// </summary>
    public static PaginationRequest Default => new();

    /// <summary>
    /// Creates a pagination request with specified values
    /// </summary>
    public static PaginationRequest Create(int page, int pageSize)
    {
        var request = new PaginationRequest { Page = page, PageSize = pageSize };
        request.Validate();
        return request;
    }
}