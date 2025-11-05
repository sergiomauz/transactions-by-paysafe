using Microsoft.AspNetCore.Mvc;


namespace Application.Commons.RequestParams
{
    public class GuidRouteRequestParam
    {
        [FromRoute(Name = "id")]
        public string? Id { get; set; }
    }
}
