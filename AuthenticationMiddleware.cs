using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

public class AuthenticationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<AuthenticationMiddleware> _logger;
    private const string AUTH_HEADER = "Authorization";
    private const string BEARER_PREFIX = "Bearer ";
    private const string VALID_TOKEN = "your-secret-token"; // Replace with your real token or validation logic

    public AuthenticationMiddleware(RequestDelegate next, ILogger<AuthenticationMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Headers.TryGetValue(AUTH_HEADER, out var authHeader) ||
            !authHeader.ToString().StartsWith(BEARER_PREFIX) ||
            authHeader.ToString().Substring(BEARER_PREFIX.Length) != VALID_TOKEN)
        {
            _logger.LogWarning("Unauthorized request to {Path}", context.Request.Path);
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            context.Response.ContentType = "application/json";
            var errorResponse = JsonSerializer.Serialize(new { error = "Unauthorized" });
            await context.Response.WriteAsync(errorResponse);
            return;
        }

        await _next(context);
    }
}
