using UserManagementAPI.Middleware;
using UserManagementAPI.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(); // Enable MVC
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Use the custom error handling middleware
app.UseMiddleware<ErrorHandlingMiddleware>();

// Use the custom token authentication middleware
app.UseMiddleware<TokenAuthenticationMiddleware>();

// Use the custom logging middleware
app.UseMiddleware<RequestLoggingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseAuthorization(); // Enable authorization middleware

// Map controllers
app.MapControllers();

app.Run();