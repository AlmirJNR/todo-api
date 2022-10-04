using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
[ApiController]
[Route("[controller]/v1")]
public class APIController : ControllerBase
{
    /// <summary>
    /// Default API check-status response
    /// </summary>
    /// <response code="200">Default API check-status response</response>
    [AllowAnonymous]
    [HttpGet]
    public IActionResult GetUnauthorized() => Ok("Welcome to the ToDo API, you're ready to go");
}