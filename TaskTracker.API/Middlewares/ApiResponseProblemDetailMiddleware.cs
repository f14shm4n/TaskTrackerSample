using TaskTracker.API.Application;

namespace TaskTracker.API.Middlewares
{
    internal sealed class ApiResponseProblemDetailMiddleware : IMiddleware
    {
        private readonly IProblemDetailsService _problemDetailsService;

        public ApiResponseProblemDetailMiddleware(IProblemDetailsService problemDetailsService)
        {
            _problemDetailsService = problemDetailsService;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {           
            var f = context.Features.Get<ApiRequestErrorFeature>();
            if (f is not null)
            {
                await _problemDetailsService.WriteAsync(new ProblemDetailsContext
                {
                    HttpContext = context,
                    ProblemDetails =
                    {
                        Title = f.Title,
                        Detail = f.Description
                    }
                });
            }

            await next(context);
        }
    }
}
