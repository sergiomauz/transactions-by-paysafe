using Microsoft.AspNetCore.Http;
using AutoMapper;
using MediatR;
using Application.Commons.Mapping;
using Application.Commons.Queries;

namespace Application.UseCases.AccountTransactions.Commands.UpdateAccountTransaction
{
    public class UpdateAccountTransactionCommand :
        GuidQuery,
        IMapFrom<HttpRequest>,
        IMapFrom<UpdateAccountTransactionRoute>,
        IMapFrom<UpdateAccountTransactionDto>,
        IRequest<UpdateAccountTransactionVm>
    {
        public int? AccountTransactionStatus { get; set; }
        public HttpRequest? Request { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<HttpRequest, UpdateAccountTransactionCommand>()
                .ForMember(d => d.Request, m => m.MapFrom(o => o));

            profile.CreateMap<UpdateAccountTransactionRoute, UpdateAccountTransactionCommand>()
                .ForMember(d => d.Id, m => m.MapFrom(o => o.Id));

            profile.CreateMap<UpdateAccountTransactionDto, UpdateAccountTransactionCommand>()
                .ForMember(d => d.AccountTransactionStatus, m => m.MapFrom(o => o.AccountTransactionStatus));
        }
    }
}
