using System.Text.Json.Serialization;

namespace Application.UseCases.AccountTransactions.Commands.CreateAccountTransaction
{
    public class CreateAccountTransactionDto
    {
        [JsonPropertyName("sourceAccountId")]
        public string? SourceAccountId { get; set; }

        [JsonPropertyName("targetAccountId")]
        public string? TargetAccountId { get; set; }

        [JsonPropertyName("transferTypeId")]
        public int? TransferTypeId { get; set; }

        [JsonPropertyName("value")]
        public decimal? Amount { get; set; }

        [JsonPropertyName("ticketValidator")]
        public long? TicketValidator { get; set; }
    }
}
