/*
    ErrorHandlingMiddleware.cs - TechHive Solutions User Management API
    ------------------------------------------------------------------
    This file defines a custom ASP.NET Core middleware for global error handling.

    Main Features:
    - Catches unhandled exceptions thrown by downstream middleware or endpoints.
    - Logs exception details for diagnostics and auditing.
    - Returns a standardized 500 Internal Server Error response in JSON format.
    - Prevents leaking sensitive exception details to clients.
    - Improves API reliability and user experience by handling errors gracefully.

    This middleware is registered in Program.cs and wraps all API requests for robust error management.
*/
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception occurred.");
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";
            var errorResponse = JsonSerializer.Serialize(new { error = "Internal server error." });
            await context.Response.WriteAsync(errorResponse);
        }
    }
}
