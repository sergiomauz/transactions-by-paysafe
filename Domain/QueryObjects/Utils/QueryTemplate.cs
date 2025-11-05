namespace Domain.QueryObjects.Utils
{
    public class QueryTemplate<TFilter, TOrder>
    {
        public TFilter? FilteringCriteria { get; set; }
        public TOrder? OrderingCriteria { get; set; }
    }
}
