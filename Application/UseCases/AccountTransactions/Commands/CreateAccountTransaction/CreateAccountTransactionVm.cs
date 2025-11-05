using System.Text.Json.Serialization;
using AutoMapper;
using Domain.Entities;
using Application.Commons.Mapping;
using Application.Commons.VMs;

namespace Application.UseCases.AccountTransactions.Commands.CreateAccountTransaction
{
    public class CreateAccountTransactionVm :
        BasicVm,
        IMapFrom<AccountTransaction>
    {
        [JsonPropertyName("transactionExternalId")]
        public string AccountTransactionId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<AccountTransaction, CreateAccountTransactionVm>()
                .ForMember(d => d.AccountTransactionId, m => m.MapFrom(o => o.Id))
                .ForMember(d => d.CreatedAt, m => m.MapFrom(o => o.CreatedAt.Value.ToString("yyyy-MM-dd HH:mm:ss")));
        }
    }
}
