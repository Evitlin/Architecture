namespace ArchitectureCatalog.API.DTOs.Common;

public class PagedResult<T>
{
    public const int DefaultPage = 1;
    public const int DefaultPageSize = 10;
    public const int MaxPageSize = 100;

    public IReadOnlyList<T> Items { get; set; } = Array.Empty<T>();
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages { get; set; }

    public static (int Page, int PageSize) Normalize(int page, int pageSize)
    {
        if (page < 1)
        {
            page = DefaultPage;
        }

        if (pageSize < 1)
        {
            pageSize = DefaultPageSize;
        }
        else if (pageSize > MaxPageSize)
        {
            pageSize = MaxPageSize;
        }

        return (page, pageSize);
    }

    public static PagedResult<T> Create(IReadOnlyList<T> items, int page, int pageSize, int totalItems)
    {
        return new PagedResult<T>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalItems = totalItems,
            TotalPages = totalItems == 0 ? 0 : (int)Math.Ceiling(totalItems / (double)pageSize)
        };
    }
}
