using System.Net;
using Data.Models;

namespace Contracts.Interfaces.Repositories;

public interface IGroupRepository
{
    public Task<(Group?, HttpStatusCode)> CreateGroup(Group groupModel);
    public Task<HttpStatusCode> DeleteGroup(Guid groupId);
    public Task<Group?> GetGroupById(Guid groupId);
    public Task<HttpStatusCode> InsertUserIntoGroup(User userModel, Guid groupId);
    public Task<HttpStatusCode> RemoveUserFromGroup(Guid userAuthServerId, Guid groupId);
    public Task<HttpStatusCode> UpdateGroup(Group groupModel);
}