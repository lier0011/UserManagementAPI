using Microsoft.AspNetCore.Mvc;
using UserManagementAPI.Models;

namespace UserManagementAPI.Controllers;

[ApiController]
[Route("users")]
public class UsersController : ControllerBase
{
    private static readonly Dictionary<int, User> Users = new()
    {
        { 1, new User { UserName = "Alice", Age = 28 } },
        { 2, new User { UserName = "Bob", Age = 34 } },
        { 3, new User { UserName = "Charlie", Age = 22 } }
    };

    [HttpGet]
    public IActionResult GetUsers([FromQuery] string? name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Ok(Users.Values);

        return Ok(Users.Values.Where(u => u.UserName.Contains(name, StringComparison.OrdinalIgnoreCase)));
    }

    [HttpGet("{id:int}")]
    public IActionResult GetUserById(int id)
    {
        ValidateId(id);

        return Users.TryGetValue(id, out var user) ? Ok(user) : NotFound();
    }

    [HttpPost]
    public IActionResult CreateUser([FromBody] User newUser)
    {
        ValidateUser(newUser);

        var newId = Users.Count > 0 ? Users.Keys.Max() + 1 : 1;
        Users.Add(newId, newUser);

        return CreatedAtAction(nameof(GetUserById), new { id = newId }, newUser);
    }

    [HttpPut("{id:int}")]
    public IActionResult UpdateUser(int id, [FromBody] User updatedUser)
    {
        ValidateId(id);
        ValidateUser(updatedUser);

        if (!Users.ContainsKey(id))
            return NotFound();

        Users[id] = updatedUser;
        return Ok(updatedUser);
    }

    [HttpDelete("{id:int}")]
    public IActionResult DeleteUser(int id)
    {
        ValidateId(id);

        if (!Users.ContainsKey(id))
            return NotFound();

        return Users.Remove(id) ? NoContent() : NotFound();
    }

    private static void ValidateUser(User user)
    {
        if (string.IsNullOrWhiteSpace(user.UserName))
            throw new ArgumentException("Username cannot be empty");

        if (user.Age <= 0)
            throw new ArgumentException("Age must be greater than 0");
    }

    private static void ValidateId(int id)
    {
        if (id <= 0)
            throw new ArgumentException("Id must be greater than 0");
    }
}
