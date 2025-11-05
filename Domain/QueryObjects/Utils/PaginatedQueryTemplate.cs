namespace Domain.QueryObjects.Utils
{
    public class PaginatedQueryTemplate<TFilter, TOrder>
    {
        public TFilter? FilteringCriteria { get; set; }
        public TOrder? OrderingCriteria { get; set; }
        public int? CurrentPage { get; set; }
        public int? PageSize { get; set; }
    }
}
