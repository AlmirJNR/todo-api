using System.Net;
using API.Utils;
using Contracts.DTOs.UserDtos;
using Contracts.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

/// <response code="401"></response>
[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    /// <param name="userId"></param>
    /// <param name="changeUserRoleDto"></param>
    /// <returns></returns>
    /// <response code="400"></response>
    /// <response code="404"></response>
    /// <response code="500"></response>
    /// <response code="200"></response>
    [HttpPatch("{userId:Guid}/role")]
    public async Task<IActionResult> ChangeUserRole(
        [FromRoute] Guid userId,
        [FromBody] ChangeUserRoleDto changeUserRoleDto)
    {
        if (changeUserRoleDto.RoleId == Guid.Empty)
            return BadRequest(string.Empty);

        var (user, actionResult) = await JWTHelper.UserExists(_userService, User);
        if (user is null && actionResult is not null)
            return actionResult;

        var adminRoleGuid = new Guid(Constants.AdminRoleGuid);
        if (!(user?.RoleId == adminRoleGuid))
            return Unauthorized(string.Empty);

        var response = await _userService.ChangeUserRole(userId, changeUserRoleDto);

        return response switch
        {
            HttpStatusCode.NotFound => NotFound(string.Empty),
            HttpStatusCode.InternalServerError => Problem(string.Empty),
            _ => Ok()
        };
    }

    /// <returns></returns>
    /// <response code="400"></response>
    /// <response code="409"></response>
    /// <response code="500"></response>
    /// <response code="201"></response>
    [HttpPost]
    public async Task<IActionResult> CreateUser()
    {
        var claimDictionary = User.Claims.ToDictionary(claim => claim.Type, claim => claim.Value);
        var claimExists = claimDictionary.TryGetValue("userId", out var userAuthServerId);
        if (!claimExists)
            return BadRequest();
        
        var guidWasParsed = Guid.TryParse(userAuthServerId, out var userAuthServerGuid);
        if (!guidWasParsed)
            return BadRequest();

        var createUserDto = new CreateUserDto
        {
            RoleId = new Guid(Constants.DefaultRoleGuid),
            AuthServerId = userAuthServerGuid
        };

        var (createdUserDto, httpStatusCode) = await _userService.CreateUser(createUserDto);

        return httpStatusCode switch
        {
            HttpStatusCode.Conflict => Conflict(string.Empty),
            HttpStatusCode.InternalServerError => Problem(string.Empty),
            _ => Created(string.Empty, createdUserDto)
        };
    }

    /// <param name="userId"></param>
    /// <returns></returns>
    /// <response code="400"></response>
    /// <response code="404"></response>
    /// <response code="500"></response>
    /// <response code="200"></response>
    [HttpDelete("{userId:Guid}")]
    public async Task<IActionResult> DeleteUser([FromRoute] Guid userId)
    {
        var claimDictionary = User.Claims.ToDictionary(claim => claim.Type, claim => claim.Value);
        var claimExists = claimDictionary.TryGetValue("userId", out var userAuthServerId);
        if (!claimExists || string.IsNullOrWhiteSpace(userAuthServerId))
            return BadRequest();

        var guidWasParsed = Guid.TryParse(userAuthServerId, out var userAuthServerGuid);
        if (!guidWasParsed)
            return BadRequest();

        if (userAuthServerGuid == userId)
        {
            var responseDefault = await _userService.DeleteUser(userId);

            return responseDefault switch
            {
                HttpStatusCode.NotFound => NotFound(string.Empty),
                HttpStatusCode.InternalServerError => Problem(string.Empty),
                _ => Ok(string.Empty)
            };
        }

        var user = await _userService.GetUserById(userAuthServerGuid);
        if (user is null)
            return NotFound();

        var adminRoleGuid = new Guid(Constants.AdminRoleGuid);
        if (user.RoleId != adminRoleGuid)
            return Unauthorized(string.Empty);

        var responseAdmin = await _userService.DeleteUser(userId);

        return responseAdmin switch
        {
            HttpStatusCode.NotFound => NotFound(string.Empty),
            HttpStatusCode.InternalServerError => Problem(string.Empty),
            _ => Ok(string.Empty)
        };
    }

    /// <param name="page"></param>
    /// <param name="includeDeleted"></param>
    /// <param name="limit"></param>
    /// <returns></returns>
    /// <response code="400"></response>
    /// <response code="404"></response>
    /// <response code="200"></response>
    [HttpGet]
    public async Task<IActionResult> GetAllUsers(
        [FromQuery] int page = 1,
        [FromQuery] bool includeDeleted = false,
        int limit = 6)
    {
        var (user, actionResult) = await JWTHelper.UserExists(_userService, User);
        if (user is null && actionResult is not null)
            return actionResult;

        var adminRoleGuid = new Guid(Constants.AdminRoleGuid);
        if (user?.RoleId != adminRoleGuid)
            return Unauthorized(string.Empty);

        var response = await _userService.GetAllUsers(includeDeleted, page, limit);

        return Ok(response);
    }
}