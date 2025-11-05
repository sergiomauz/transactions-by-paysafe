using Microsoft.AspNetCore.Mvc;


namespace Application.Commons.RequestParams
{
    public class IdRouteRequestParam
    {
        [FromRoute(Name = "id")]
        public int? Id { get; set; }
    }
}
