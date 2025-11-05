using System.Text.Json.Serialization;


namespace Application.Commons.VMs
{
    public class WasUpdatedVm
    {
        [JsonPropertyName("was_updated")]
        public bool WasUpdated { get; set; }
    }
}
