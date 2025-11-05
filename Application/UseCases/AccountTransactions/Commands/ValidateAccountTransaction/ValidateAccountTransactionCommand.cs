using AutoMapper;
using MediatR;
using Application.Commons.Mapping;

namespace Application.UseCases.AccountTransactions.Commands.ValidateAccountTransaction
{
    public class ValidateAccountTransactionCommand :
        IMapFrom<CancellationToken>,
        IRequest
    {
        public CancellationToken StoppingToken { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CancellationToken, ValidateAccountTransactionCommand>()
                .ForMember(d => d.StoppingToken, m => m.MapFrom(o => o));
        }
    }
}