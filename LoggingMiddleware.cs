using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

/*
    LoggingMiddleware.cs - TechHive Solutions User Management API
    ------------------------------------------------------------
    This file defines a custom ASP.NET Core middleware for request and response logging.

    Main Features:
    - Logs details of all incoming HTTP requests (method and path).
    - Logs details of all outgoing HTTP responses (method, path, and status code).
    - Uses ASP.NET Core's built-in logging infrastructure for structured logs.
    - Helps with monitoring, debugging, and auditing API activity.

    This middleware is registered in Program.cs and wraps all API requests for comprehensive logging.
*/
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

public class LoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<LoggingMiddleware> _logger;

    public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Log incoming request
        var method = context.Request.Method;
        var path = context.Request.Path;
        _logger.LogInformation("Incoming Request: {Method} {Path}", method, path);

        await _next(context);

        // Log outgoing response
        var statusCode = context.Response.StatusCode;
        _logger.LogInformation("Outgoing Response: {Method} {Path} - {StatusCode}", method, path, statusCode);
    }
}
