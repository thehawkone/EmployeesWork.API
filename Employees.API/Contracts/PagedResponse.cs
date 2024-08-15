namespace Employees.API.Contracts;

public record PagedResponse<T>(
    int PageNumber,
    int PageSize,
    int TotalItems,
    int TotalPages,
        List<T> Items
    );
    
    