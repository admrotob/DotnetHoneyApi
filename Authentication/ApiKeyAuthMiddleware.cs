namespace DotnetHoneyApi.Authentication
{
    public class ApiKeyAuthMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ApiKeyAuthMiddleware> _logger;

        public ApiKeyAuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            // Ignore health check requests because they need to be allowed to pass through
            if (context.Request.Path.StartsWithSegments("/healthz"))
            {
                await _next(context);
                return;
            }
            else
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("The Key or Secret Value provided was incorrect. Please check your headers.");
                return;
            }
        }
    }
}
