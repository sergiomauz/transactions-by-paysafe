using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;


namespace Api.Responses
{
    public class OkResponsesProcess : ResultFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            var response = new CustomOkResponse<object>(
                message: null,
                data: ((ObjectResult)context.Result).Value
            );

            context.Result = new ObjectResult(response)
            {
                StatusCode = StatusCodes.Status200OK
            };
        }
    }
}
