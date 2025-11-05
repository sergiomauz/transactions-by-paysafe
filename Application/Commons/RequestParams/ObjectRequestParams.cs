using System.Text.Json.Serialization;


namespace Application.Commons.RequestParams
{
    public class ObjectRequestParams<TFilter, TOrder> : PaginatedRequestParams
    {
        [JsonPropertyName("filtering_criteria")]
        public TFilter? FilteringCriteria { get; set; }

        [JsonPropertyName("ordering_criteria")]
        public TOrder? OrderingCriteria { get; set; }
    }
}
