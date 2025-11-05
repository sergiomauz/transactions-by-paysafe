using System.Text.Json.Serialization;


namespace Application.Commons.VMs
{
    public class WereDeletedVm
    {
        [JsonPropertyName("were_deleted")]
        public bool WereDeleted { get; set; }

        [JsonPropertyName("total_affected")]
        public int TotalAffected { get; set; }
    }
}
