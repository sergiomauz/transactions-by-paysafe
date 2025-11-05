using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;


namespace Application.Commons.RequestParams
{
    public class BasicSearchRequestParams : PaginatedRequestParams
    {
        [FromQuery(Name = "text_filter"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? TextFilter { get; set; }
    }
}
