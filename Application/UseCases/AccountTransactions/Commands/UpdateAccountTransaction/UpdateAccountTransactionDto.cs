using System.Text.Json.Serialization;

namespace Application.UseCases.AccountTransactions.Commands.UpdateAccountTransaction
{
    public class UpdateAccountTransactionDto
    {
        [JsonPropertyName("accountTransactionStatus")]
        public int? AccountTransactionStatus { get; set; }
    }
}
