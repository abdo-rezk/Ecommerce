namespace Ecommerce.Helper
{
    public class Pagination<T> where T : class
    {
        /*int _pageIndex;
        int _pageSize;
        public int PageIndex
        {
            get => _pageIndex;
            set => _pageIndex = value < 1 ? 1 : value;
        }
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value < 1 ? 1 : value;
        }
        public int TotalPages => (int)Math.Ceiling((double)Count / PageSize);
        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;
        public Pagination() { }*/
        public int _pageSize { get; set; }
        public int _pageIndex { get; set; }
        public int Count { get; set; }
        public IReadOnlyList<T> Data { get; set; }
        public Pagination(int pageIndex, int pageSize, int count, IReadOnlyList<T> data)
        {
            _pageIndex = pageIndex;
            _pageSize = pageSize;
            Count = count;
            Data = data;
        }
    }
}
