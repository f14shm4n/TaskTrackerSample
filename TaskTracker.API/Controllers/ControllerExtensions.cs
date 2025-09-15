using Microsoft.AspNetCore.Mvc;
using System.Net;
using TaskTracker.API.Application;

namespace TaskTracker.API.Controllers
{
    public static class ControllerExtensions
    {
        public static ActionResult ToActionResultResult(this ControllerBase controller, ApiRequestResult result)
        {
            ArgumentNullException.ThrowIfNull(result, $"{nameof(result)}");

            switch (result.StatusCode)
            {
                case HttpStatusCode.OK:
                    return controller.Ok(result.Value);
                case var successCode when (int)successCode >= 201 && (int)successCode <= 399:
                    return controller.StatusCode((int)successCode);
                case var errorCode when (int)errorCode >= 400 && (int)errorCode <= 599:
                    controller.HttpContext.Features.Set(result.Error);
                    return controller.StatusCode((int)errorCode);
                default:
                    throw new NotImplementedException($"Unsupported HTTP status code. StatusCode: {result.StatusCode}");
            }
        }
    }
}
