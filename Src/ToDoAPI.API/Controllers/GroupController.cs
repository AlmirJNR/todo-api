using System.Net;
using API.Utils;
using Contracts.DTOs.GroupDtos;
using Contracts.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

/// <response code="401"></response>
[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
public class GroupController : ControllerBase
{
    private readonly IGroupService _groupService;
    private readonly IUserService _userService;

    public GroupController(IGroupService groupService, IUserService userService)
    {
        _groupService = groupService;
        _userService = userService;
    }

    /// <param name="groupId"></param>
    /// <param name="changeGroupAdminDto"></param>
    /// <returns></returns>
    /// <response code="400"></response>
    /// <response code="404"></response>
    /// <response code="500"></response>
    /// <response code="200"></response>
    [HttpPatch("{groupId:Guid}/admin")]
    public async Task<IActionResult> ChangeGroupAdmin(
        [FromRoute] Guid groupId,
        [FromBody] ChangeGroupAdminDto changeGroupAdminDto)
    {
        if (changeGroupAdminDto.Admin == Guid.Empty)
            return BadRequest("New admin guid must not be empty");

        var (user, actionResult) = await JWTHelper.UserExists(_userService, User);
        if (user is null && actionResult is not null)
            return actionResult;

        var group = user?.Groups.FirstOrDefault(group => group.Id == groupId);
        if (group is null)
            return NotFound();

        var adminRoleGuid = new Guid(Constants.AdminRoleGuid);
        if (user?.RoleId != adminRoleGuid || user.AuthServerId != group.Admin)
            return Unauthorized();

        var response = await _groupService.ChangeGroupAdmin(groupId, changeGroupAdminDto);
        return response switch
        {
            HttpStatusCode.NotFound => NotFound(string.Empty),
            HttpStatusCode.InternalServerError => Problem(string.Empty),
            _ => Ok()
        };
    }

    /// <param name="groupId"></param>
    /// <param name="changeGroupNameDto"></param>
    /// <returns></returns>
    /// <response code="400"></response>
    /// <response code="404"></response>
    /// <response code="500"></response>
    /// <response code="200"></response>
    [HttpPatch("{groupId:Guid}/name")]
    public async Task<IActionResult> ChangeGroupName(
        [FromRoute] Guid groupId,
        [FromBody] ChangeGroupNameDto changeGroupNameDto)
    {
        if (string.IsNullOrWhiteSpace(changeGroupNameDto.Name))
            return BadRequest("Name must not be null or empty space");

        var (user, actionResult) = await JWTHelper.UserExists(_userService, User);
        if (user is null && actionResult is not null)
            return actionResult;

        var group = user?.Groups.FirstOrDefault(group => group.Id == groupId);
        if (group is null)
            return NotFound();

        var adminRoleGuid = new Guid(Constants.AdminRoleGuid);
        if (user?.RoleId != adminRoleGuid || user.AuthServerId != group.Admin)
            return Unauthorized();

        var response = await _groupService.ChangeGroupName(groupId, changeGroupNameDto);
        return response switch
        {
            HttpStatusCode.NotFound => NotFound(string.Empty),
            HttpStatusCode.InternalServerError => Problem(string.Empty),
            _ => Ok()
        };
    }

    public readonly record struct CreateGroupInitDto(string Name);

    /// <param name="createGroupInitDto"></param>
    /// <returns></returns>
    /// <response code="500"></response>
    /// <response code="201"></response>
    [HttpPost]
    public async Task<IActionResult> CreateGroup([FromBody] CreateGroupInitDto createGroupInitDto)
    {
        if (string.IsNullOrWhiteSpace(createGroupInitDto.Name))
            return BadRequest("Name must not be null or empty space");

        var (user, actionResultUserExists) = await JWTHelper.UserExists(_userService, User);
        if (user is null && actionResultUserExists is not null)
            return actionResultUserExists;

        var (claimValue, _) = JWTHelper.ClaimExists(User, "userId");
        if (string.IsNullOrWhiteSpace(claimValue))
            return BadRequest();

        var createGroupDto = new CreateGroupDto
        {
            Admin = new Guid(claimValue),
            CreatedBy = new Guid(claimValue),
            Name = createGroupInitDto.Name
        };

        var (createdGroupDto, statusCode) = await _groupService.CreateGroup(createGroupDto);

        return statusCode switch
        {
            HttpStatusCode.InternalServerError => Problem(string.Empty),
            _ => Created(string.Empty, createdGroupDto)
        };
    }

    /// <param name="groupId"></param>
    /// <returns></returns>
    /// <response code="404"></response>
    /// <response code="500"></response>
    /// <response code="200"></response>
    [HttpDelete]
    public async Task<IActionResult> DeleteGroup([FromRoute] Guid groupId)
    {
        var (user, actionResultUserExists) = await JWTHelper.UserExists(_userService, User);
        if (user is null && actionResultUserExists is not null)
            return actionResultUserExists;

        var group = await _groupService.GetGroupById(groupId);
        if (group is null)
            return NotFound();

        var userIsAdminOfGroup = user?.AuthServerId == group.Admin;
        if (!userIsAdminOfGroup)
            return Unauthorized();

        var response = await _groupService.DeleteGroup(groupId);

        return response switch
        {
            HttpStatusCode.NotFound => NotFound(),
            HttpStatusCode.InternalServerError => Problem(),
            _ => Ok()
        };
    }

    /// <param name="groupId"></param>
    /// <returns></returns>
    /// <response code="404"></response>
    /// <response code="200"></response>
    [HttpGet]
    public async Task<IActionResult> GetGroupById([FromRoute] Guid groupId)
    {
        var (user, actionResultUserExists) = await JWTHelper.UserExists(_userService, User);
        if (user is null && actionResultUserExists is not null)
            return actionResultUserExists;

        var group = await _groupService.GetGroupById(groupId);
        if (group is null)
            return NotFound();

        var adminRoleGuid = new Guid(Constants.AdminRoleGuid);
        if (!(user?.AuthServerId == adminRoleGuid && !group.Users.Contains(user)))
            return Unauthorized();

        var userIsAMemberOf = group.Users.Contains(user);
        if (!userIsAMemberOf)
            return Unauthorized();

        return Ok(group);
    }

    /// <param name="groupId"></param>
    /// <param name="insertUserIntoGroupDto"></param>
    /// <returns></returns>
    [HttpPost("{groupId:Guid}/new-user")]
    public async Task<IActionResult> InsertUserIntoGroup(
        [FromRoute] Guid groupId,
        [FromBody] InsertUserIntoGroupDto insertUserIntoGroupDto)
    {
        
    }

    /// <param name="groupId"></param>
    /// <param name="removeUserFromGroupDto"></param>
    /// <returns></returns>
    [HttpPost("{groupId:Guid}/remove-user")]
    public async Task<IActionResult> RemoveUserFromGroup(
        [FromRoute] Guid groupId,
        [FromBody] RemoveUserFromGroupDto removeUserFromGroupDto)
    {
        
    }
}