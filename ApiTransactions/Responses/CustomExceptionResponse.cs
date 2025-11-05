using System.Text.Json.Serialization;


namespace Api.Responses
{
    public class CustomExceptionResponse
    {
        [JsonPropertyName("message"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Message { get; set; }

        [JsonPropertyName("exceptions"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Dictionary<string, IEnumerable<Dictionary<string, string>>> Exceptions { get; set; }

        public CustomExceptionResponse(string? message, Dictionary<string, IEnumerable<Dictionary<string, string>>>? exceptions = null)
        {
            Message = message;
            Exceptions = exceptions;
        }
    }
}
