namespace Application.Commons.Queries
{
    public class BasicSearchQuery : PaginatedQuery
    {
        public string? TextFilter { get; set; }
    }
}
