namespace Catalog.API.Model;

public record PaginatedItems<T>(int PageIndex, int PageSize, long Count, IEnumerable<T> Data);