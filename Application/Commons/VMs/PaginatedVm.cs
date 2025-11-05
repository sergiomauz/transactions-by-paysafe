using System.Text.Json.Serialization;


namespace Application.Commons.VMs
{

    public class PaginatedVm<T>
    {
        [JsonPropertyName("items")]
        public IEnumerable<T> Items { get; }

        [JsonPropertyName("total_items")]
        public int TotalItems { get; }

        [JsonPropertyName("current_page")]
        public int CurrentPage { get; }

        [JsonPropertyName("page_size")]
        public int PageSize { get; }

        [JsonPropertyName("total_pages")]
        public int TotalPages { get; }

        public PaginatedVm(IEnumerable<T> items, int totalItems, int currentPage, int pageSize)
        {
            TotalItems = totalItems;
            CurrentPage = currentPage;
            if (pageSize > 0)
            {
                PageSize = pageSize;

                TotalPages = (int)Math.Ceiling(totalItems / (decimal)pageSize);
                if (TotalPages == 0) TotalPages = 1;
            }
            else
            {
                PageSize = totalItems;

                TotalPages = 1;
            }
            Items = items;
        }
    }
}
