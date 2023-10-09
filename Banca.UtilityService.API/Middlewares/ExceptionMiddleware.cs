using System.Net;
using Serilog;

namespace Banca.UtilityService.API;
public class ExceptionMiddleware
{
    private readonly RequestDelegate request;
    private readonly ILogger<ExceptionMiddleware> logger;
    private readonly IDiagnosticContext context;
    public ExceptionMiddleware(RequestDelegate request, ILogger<ExceptionMiddleware> logger, IDiagnosticContext context)
    {
        this.request = request;
        this.logger = logger;
        this.context = context;
    }

    public async Task Invoke(HttpContext httpContext)
    {
        try
        {
            await request(httpContext);
        }
        catch (Exception ex)
        {
            if (ex.InnerException != null)
            {
                logger.LogError("{ExceptionType} {ExceptionMessage}", ex.InnerException.GetType().ToString(), ex.InnerException.Message);
            }
            else
            {
                logger.LogError("{ExceptionType} {ExceptionMessage}", ex.GetType().ToString(), ex.Message);
            }
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await httpContext.Response.WriteAsync("An Error occurred, please try again later.");
        }
    }
}

public static class ExceptionMiddlewareExtension
{
    public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionMiddleware>();
        return app;
    }
}
