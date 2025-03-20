using UserManagementAPI.Middleware;
using UserManagementAPI.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
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

// In-memory user list with initial data
var users = new Dictionary<int, User>
{
    { 1, new User { UserName = "Alice", Age = 28 }},
    { 2, new User { UserName = "Bob", Age = 34 }},
    { 3, new User { UserName = "Charlie", Age = 22 }}
};

// CRUD Endpoints
app.MapGet("/users", (string? name) => {
    if (string.IsNullOrWhiteSpace(name))
        return Results.Ok(users.Values);

    return Results.Ok(users.Values.Where(u => u.UserName.Contains(name, StringComparison.OrdinalIgnoreCase)));
});

void validateUser(User user)
{
    if (string.IsNullOrWhiteSpace(user.UserName))
        throw new ArgumentException("Username cannot be empty");

    if (user.Age <= 0)
        throw new ArgumentException("Age must be greater than 0");
}

void validateId(int id)
{
    if (id <= 0)
        throw new ArgumentException("Id must be greater than 0");
}

app.MapGet("/users/{id:int}", (int id) =>
{
    validateId(id);

    return users.TryGetValue(id, out var user) ? Results.Ok(user) : Results.NotFound();
});

app.MapPost("/users", (User newUser) =>
{
    validateUser(newUser);

    var newId = users.Count > 0 ? users.Keys.Max() + 1 : 1;
    users.Add(newId, newUser);

    return Results.Created($"/users/{newId}", newUser);
});

app.MapPut("/users/{id:int}", (int id, User updatedUser) =>
{
    validateId(id);
    validateUser(updatedUser);

    if (!users.ContainsKey(id))
        return Results.NotFound();

    users[id] = updatedUser;
    return Results.Ok(updatedUser);
});

app.MapDelete("/users/{id:int}", (int id) =>
{
    validateId(id);

    if (!users.ContainsKey(id))
        return Results.NotFound();

    return users.Remove(id) ? Results.NoContent() : Results.NotFound();
});

app.MapGet("/", () => "Hello World!");

app.Run();