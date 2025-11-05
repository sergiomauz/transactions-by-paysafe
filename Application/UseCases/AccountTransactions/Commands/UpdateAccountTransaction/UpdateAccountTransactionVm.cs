using System.Text.Json.Serialization;
using AutoMapper;
using Domain.Entities;
using Application.Commons.Mapping;
using Application.Commons.VMs;

namespace Application.UseCases.AccountTransactions.Commands.UpdateAccountTransaction
{
    public class UpdateAccountTransactionVm :
        BasicVm,
        IMapFrom<AccountTransaction>
    {
        [JsonPropertyName("accountTransactionId")]
        public string AccountTransactionId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<AccountTransaction, UpdateAccountTransactionVm>()
                .ForMember(d => d.AccountTransactionId, m => m.MapFrom(o => o.Id))
                .ForMember(d => d.CreatedAt, m => m.MapFrom(o => o.CreatedAt.Value.ToString("yyyy-MM-dd HH:mm:ss")))
                .ForMember(d => d.ModifiedAt, m => m.MapFrom(o => o.ModifiedAt.Value.ToString("yyyy-MM-dd HH:mm:ss")));
        }
    }
}
