using System.Text.Json.Serialization;


namespace Application.Commons.VMs
{
    public class ResponseMessageVm
    {
        [JsonPropertyName("message"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Message { get; set; }
    }
}
