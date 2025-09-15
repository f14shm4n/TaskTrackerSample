using System.Net;

namespace TaskTracker.API.Application
{
    public class ApiRequestResult
    {
        public HttpStatusCode StatusCode { get; init; }
        public ApiResponseValue? Value { get; init; }
        public ApiRequestErrorFeature? Error { get; init; }

        private ApiRequestResult()
        {
        }

        public static ApiRequestResult Success()
        {
            return new ApiRequestResult
            {
                StatusCode = HttpStatusCode.OK,
                Value = new ApiResponseValue(true)
            };
        }

        public static ApiRequestResult Success<TPayload>(TPayload payload) where TPayload : class
        {
            return new ApiRequestResult
            {
                StatusCode = HttpStatusCode.OK,
                Value = new ApiResponseValue<TPayload>(payload)
            };
        }

        public static ApiRequestResult Fail(HttpStatusCode errorCode, string? title = null, string? description = null)
        {
            return new ApiRequestResult
            {
                StatusCode = errorCode,
                Error = new ApiRequestErrorFeature(title, description)
            };
        }

        public static ApiRequestResult NotFound(string? title = null, string? description = null) => Fail(HttpStatusCode.NotFound, title, description);

        public static ApiRequestResult BadRequest(string? title = null, string? description = null) => Fail(HttpStatusCode.BadRequest, title, description);

        public static ApiRequestResult InternalServerError(string? title = null, string? description = null) => Fail(HttpStatusCode.InternalServerError, title, description);
    }
}