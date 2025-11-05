using Microsoft.AspNetCore.Mvc;


namespace Application.Commons.RequestParams
{
    public class CodeRouteRequestParam
    {
        [FromRoute(Name = "code")]
        public string? Code { get; set; }
    }
}
