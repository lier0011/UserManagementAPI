using Microsoft.AspNetCore.Mvc;

namespace UserManagementAPI.Controllers;

[ApiController]
[Route("/")]
public class HomeController : ControllerBase
{
    [HttpGet]
    public IActionResult Index()
    {
        return Ok("Hello World!");
    }
}
