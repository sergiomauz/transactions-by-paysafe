using System.Text.Json.Serialization;


namespace Application.Commons.RequestParams
{
    public class IdsBodyRequestParam
    {
        [JsonPropertyName("ids")]
        public List<int>? Ids { get; set; }
    }
}
