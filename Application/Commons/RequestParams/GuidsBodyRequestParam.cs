using System.Text.Json.Serialization;


namespace Application.Commons.RequestParams
{
    public class GuidsBodyRequestParam
    {
        [JsonPropertyName("ids")]
        public List<Guid>? Ids { get; set; }
    }
}
