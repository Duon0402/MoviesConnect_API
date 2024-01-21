using PagedList;

namespace API.Helpers.Pagination
{
    public class PagedResults<T>
    {
        public int TotalItems { get; set; }
        public IPagedList<T> PagedItems { get; set; }
    }
}
