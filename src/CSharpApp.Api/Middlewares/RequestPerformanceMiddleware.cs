namespace CSharpApp.Api.Middlewares
{
    public class RequestPerformanceMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestPerformanceMiddleware> _logger;

        public RequestPerformanceMiddleware(RequestDelegate next, ILogger<RequestPerformanceMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();
            await _next(context);
            stopwatch.Stop();
            var time = stopwatch.ElapsedMilliseconds;
            _logger.LogInformation("Request [{method}] {url} executed in {elapsed} ms", context.Request.Method, context.Request.Path, time);
        }
    }
}
