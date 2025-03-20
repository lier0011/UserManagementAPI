using System.Net;

namespace UserManagementAPI.Middleware;

public class TokenAuthenticationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<TokenAuthenticationMiddleware> _logger;
    private const string TokenHeader = "Authorization";
    private const string ValidToken = "Bearer my-secret-token"; // Replace with your token

    public TokenAuthenticationMiddleware(RequestDelegate next, ILogger<TokenAuthenticationMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Headers.TryGetValue(TokenHeader, out var token) || token != ValidToken)
        {
            _logger.LogWarning($"Unauthorized access attempt. {context.Connection.RemoteIpAddress} Path: {context.Request.Path}, Token: {token}");
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            await context.Response.WriteAsync("Unauthorized: Invalid or missing token.");
            return;
        }

        await _next(context);
    }
}
