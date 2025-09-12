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
                HttpStatusCode.InternalServerError => controller.StatusCode((int)response.StatusCode, response),
                _ => throw new NotImplementedException($"Not supported status code. HttpStatusCode: '{response.StatusCode}'"),
            };
        }
    }
}
