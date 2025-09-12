using System.Net;
using System.Text.Json.Serialization;

namespace TaskTracker.API.Application
{
    public class ApiResponseBase
    {
        public ApiResponseBase() { }

        public ApiResponseBase(bool success, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            Success = success;
            StatusCode = statusCode;
        }

        public ApiResponseBase(string error, HttpStatusCode statusCode)
        {
            Success = false;
            Error = error;
            StatusCode = statusCode;
        }

        public bool Success { get; init; }
        public string? Error { get; init; }
        [JsonIgnore]
        public HttpStatusCode? StatusCode { get; init; }
    }
}
