using PagedList;

namespace API.Helpers.Pagination
{
    public class IPagedResult<T>
    {
        public int TotalItems { get; set; }
        public IPagedList<T> PagedItems { get; set; }
    }
}