using System.Net;
using Contracts.DTOs.GroupDtos;
using Contracts.Interfaces.Repositories;
using Contracts.Interfaces.Services;
using Data.Models;

namespace API.Services;

public class GroupService : IGroupService
{
    private readonly IGroupRepository _groupRepository;
    private readonly IUserRepository _userRepository;

    public GroupService(IGroupRepository groupRepository, IUserRepository userRepository)
    {
        _groupRepository = groupRepository;
        _userRepository = userRepository;
    }

    public Task<HttpStatusCode> ChangeGroupAdmin(Guid groupId, ChangeGroupAdminDto changeGroupAdminDto)
    {
        var groupModel = changeGroupAdminDto.ToModel();
        groupModel.Id = groupId;

        return _groupRepository.UpdateGroup(groupModel);
    }

    public Task<HttpStatusCode> ChangeGroupName(Guid groupId, ChangeGroupNameDto changeGroupNameDto)
    {
        var groupModel = changeGroupNameDto.ToModel();
        groupModel.Id = groupId;

        return _groupRepository.UpdateGroup(groupModel);
    }

    public async Task<(CreatedGroupDto?, HttpStatusCode)> CreateGroup(CreateGroupDto createGroupDto)
    {
        var (groupModel, httpStatusCode) = await _groupRepository.CreateGroup(createGroupDto.ToModel());

        if (groupModel is null)
            return (null, HttpStatusCode.InternalServerError);

        return (CreatedGroupDto.FromModel(groupModel), httpStatusCode);
    }

    public Task<HttpStatusCode> DeleteGroup(Guid groupId)
        => _groupRepository.DeleteGroup(groupId);

    public Task<Group?> GetGroupById(Guid groupId)
        => _groupRepository.GetGroupById(groupId);

    public async Task<HttpStatusCode> InsertUserIntoGroup(Guid userAuthServerId, Guid groupId)
    {
        var userModel = await _userRepository.GetUserById(userAuthServerId);

        if (userModel is null)
            return HttpStatusCode.NotFound;

        return await _groupRepository.InsertUserIntoGroup(userModel, userAuthServerId);
    }

    public Task<HttpStatusCode> RemoveUserFromGroup(Guid userAuthServerId, Guid groupId)
        => _groupRepository.RemoveUserFromGroup(userAuthServerId, groupId);
}