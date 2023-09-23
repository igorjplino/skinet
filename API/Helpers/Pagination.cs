using API.Dtos;

namespace API.Helpers;

public class Pagination<T> where T : class
{
    public Pagination(
        int pageIndex,
        int pageSize,
        int totalItems,
        IReadOnlyCollection<T> data)
    {
        PageIndex = pageIndex;
        PageSize = pageSize;
        Count = totalItems;
        Data = data;
    }

    public int PageIndex { get; private set; }
    public int PageSize { get; private set; }
    public int Count { get; private set; }
    public IReadOnlyCollection<T> Data { get; private set; }
}
