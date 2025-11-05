using System.Text.Json.Serialization;


namespace Api.Responses
{
    public class CustomOkResponse<T>
    {
        [JsonPropertyName("message"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Message { get; set; }

        [JsonPropertyName("data"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public T Data { get; set; }

        public CustomOkResponse(string? message, T data)
        {
            Message = message;
            Data = data;
        }
    }
}
