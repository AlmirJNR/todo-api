using System.Net;
using Contracts.DTOs.GroupDtos;
using Data.Models;

namespace Contracts.Interfaces.Services;

public interface IGroupService
{
    public Task<HttpStatusCode> ChangeGroupAdmin(Guid groupId, ChangeGroupAdminDto changeGroupAdminDto);
    public Task<HttpStatusCode> ChangeGroupName(Guid groupId, ChangeGroupNameDto changeGroupNameDto);
    public Task<(CreatedGroupDto?, HttpStatusCode)> CreateGroup(CreateGroupDto createGroupDto);
    public Task<HttpStatusCode> DeleteGroup(Guid groupId);
    public Task<Group?> GetGroupById(Guid groupId);
    public Task<HttpStatusCode> InsertUserIntoGroup(Guid userAuthServerId, Guid groupId);
    public Task<HttpStatusCode> RemoveUserFromGroup(Guid userAuthServerId, Guid groupId);
}