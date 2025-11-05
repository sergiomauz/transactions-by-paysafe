using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;


namespace Application.Commons.RequestParams
{
    public class PaginatedRequestParams
    {
        [FromQuery(Name = "current_page"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("current_page")]
        public int? CurrentPage { get; set; }

        [FromQuery(Name = "page_size"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("page_size")]
        public int? PageSize { get; set; }
    }
}
