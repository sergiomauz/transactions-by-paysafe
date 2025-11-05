using Microsoft.AspNetCore.Http;
using AutoMapper;
using MediatR;
using Application.Commons.Mapping;

namespace Application.UseCases.AccountTransactions.Commands.CreateAccountTransaction
{
    public class CreateAccountTransactionCommand :
        IMapFrom<HttpRequest>,
        IMapFrom<CreateAccountTransactionDto>,
        IRequest<CreateAccountTransactionVm>
    {
        public string? SourceAccountId { get; set; }
        public string? TargetAccountId { get; set; }
        public int? TransferTypeId { get; set; }
        public decimal? Amount { get; set; }
        public long? TicketValidator { get; set; }
        public HttpRequest? Request { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<HttpRequest, CreateAccountTransactionCommand>()
                .ForMember(d => d.Request, m => m.MapFrom(o => o));

            profile.CreateMap<CreateAccountTransactionDto, CreateAccountTransactionCommand>()
                .ForMember(d => d.SourceAccountId, m => m.MapFrom(o => o.SourceAccountId))
                .ForMember(d => d.TargetAccountId, m => m.MapFrom(o => o.TargetAccountId))
                .ForMember(d => d.TransferTypeId, m => m.MapFrom(o => o.TransferTypeId))
                .ForMember(d => d.Amount, m => m.MapFrom(o => o.Amount))
                .ForMember(d => d.TicketValidator, m => m.MapFrom(o => o.TicketValidator));
        }
    }
}
