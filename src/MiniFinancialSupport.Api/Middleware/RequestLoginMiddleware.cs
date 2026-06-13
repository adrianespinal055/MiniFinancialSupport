namespace MiniFinancialSupport.Api.Middleware
{
    public class RequestLoginMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoginMiddleware> _logger;

        public RequestLoginMiddleware(RequestDelegate next, ILogger<RequestLoginMiddleware>logger)
        {
            _next = next;
            _logger = logger; 
        }
       
        public async Task InvokeAsync(HttpContext context)
        {
            //Antes de pasar al siguiente a la IDA
            _logger.LogInformation("-->{Method} {Path}", context.Request.Method, context.Request.Path);

            await _next(context);//Pasar al siguiente middleware controlller

            _logger.LogInformation("--> {StatusCode}",context.Response.StatusCode);

        }
    }
}
