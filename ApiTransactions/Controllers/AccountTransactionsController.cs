using Microsoft.AspNetCore.Mvc;
using Application.UseCases.AccountTransactions.Commands.CreateAccountTransaction;
using Application.UseCases.AccountTransactions.Commands.UpdateAccountTransaction;
using Api.AccountTransactions.Authorizations.ApiKeyProtection;

namespace Api.Controllers
{
    [Route("api/account-transactions")]
    public class AccountTransactionsController : CustomControllerBase
    {
        [HttpPost("")]
        [ApiKeyProtection]
        public async Task<ActionResult<CreateAccountTransactionVm>> CreateTransaction([FromBody] CreateAccountTransactionDto body)
        {
            var command = Mapper.Map<CreateAccountTransactionCommand>(body);
            Mapper.Map(Request, command);

            var vm = await Mediator.Send(command);

            return Ok(vm);
        }

        [HttpPut("{id}")]
        [ApiKeyProtection]
        public async Task<ActionResult<UpdateAccountTransactionVm>> UpdateTransaction([FromRoute] UpdateAccountTransactionRoute route, [FromBody] UpdateAccountTransactionDto body)
        {
            var command = Mapper.Map<UpdateAccountTransactionCommand>(route);
            Mapper.Map(body, command);
            Mapper.Map(Request, command);

            var vm = await Mediator.Send(command);

            return Ok(vm);
        }
    }
}
