using Microsoft.AspNetCore.Mvc;
using System.Net;
using TaskTracker.API.Application;

namespace TaskTracker.API.Controllers
{
    public static class ControllerExtensions
    {
        public static ActionResult<T> ToActionResultResult<T>(this ControllerBase controller, T response) where T : ApiResponseBase
        {
            return response.StatusCode switch
            {
                HttpStatusCode.OK => controller.Ok(response),
                HttpStatusCode.BadRequest => controller.BadRequest(response),
                HttpStatusCode.NotFound => controller.NotFound(response),
                not null => controller.StatusCode((int)response.StatusCode, response),
                _ => throw new InvalidOperationException("The HTTP status code must be set."),
            };
        }
    }
}
