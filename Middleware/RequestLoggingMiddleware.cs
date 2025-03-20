namespace UserManagementAPI.Middleware;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;

    public RequestLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var ipAddress = context.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
        var method = context.Request.Method;
        var path = context.Request.Path;

        await _next(context); // Call the next middleware in the pipeline

        var statusCode = context.Response.StatusCode;
        Console.WriteLine($"IP: {ipAddress}, Method: {method}, Path: {path}, Status Code: {statusCode}");
    }
}
