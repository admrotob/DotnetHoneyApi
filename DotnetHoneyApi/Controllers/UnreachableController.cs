using Microsoft.AspNetCore.Mvc;

namespace DotnetHoneyApi.Controllers;

[ApiController]
[Route("/v1/[controller]")]
public class UnreachableController : Controller
{
    [HttpGet]
    public async Task<IActionResult> GetUnreachableAsync()
    {
        return Ok();
    }
}
