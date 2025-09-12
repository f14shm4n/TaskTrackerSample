using System.Net;

namespace TaskTracker.API.Application
{
    public class ApiResponseBase<TPayload> : ApiResponseBase
        where TPayload : class
    {
        public ApiResponseBase() { }

        public ApiResponseBase(bool success, HttpStatusCode statusCode = HttpStatusCode.OK) : base(success, statusCode) { }

        public ApiResponseBase(string error, HttpStatusCode statusCode) : base(error, statusCode) { }

        public ApiResponseBase(TPayload payload, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            Payload = payload;
            Success = true;
            StatusCode = statusCode;
        }

        public TPayload? Payload { get; init; }
    }
}
