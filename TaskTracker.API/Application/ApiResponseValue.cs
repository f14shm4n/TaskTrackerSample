namespace TaskTracker.API.Application
{
    public class ApiResponseValue
    {
        public ApiResponseValue() { }

        public ApiResponseValue(bool success)
        {
            Success = success;
        }

        public bool Success { get; init; }
    }

    public class ApiResponseValue<TPayload> : ApiResponseValue
    where TPayload : class
    {
        public ApiResponseValue() { }

        public ApiResponseValue(TPayload payload)
        {
            Payload = payload;
            Success = true;
        }

        public TPayload? Payload { get; init; }
    }
}