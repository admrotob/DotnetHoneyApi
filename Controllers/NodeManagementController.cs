using Microsoft.AspNetCore.Mvc;

namespace DotnetHoneyApi.Controllers;

[ApiController]
[Route("/v1/[controller]")]
public class NodeManagementController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateNodeAsync(string[] request)
    {
        if(!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var response = request;

        return CreatedAtAction(
            nameof(GetNodeAsync),
            response);
    }

    [HttpGet]
    public async Task<IActionResult> GetNodeAsync()
    {
        var octetOne = new Random().Next(50, 250);
        var octetTwo = new Random().Next(3, 250);
        return Ok($"Node is healthy\r\nIP Address: 10.10.{octetOne}.{octetTwo}\r\n");
    }
}
