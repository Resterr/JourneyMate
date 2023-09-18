namespace JourneyMate.Domain.Common.Interfaces;

public interface IPaginatedList<T>
{
    bool HasNextPage { get; }
    bool HasPreviousPage { get; }
    List<T> Items { get; }
    int PageNumber { get; }
    int TotalCount { get; }
    int TotalPages { get; }
}