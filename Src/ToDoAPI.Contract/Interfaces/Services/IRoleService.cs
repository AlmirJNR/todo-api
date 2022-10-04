using System.Net;
using Contracts.DTOs.RoleDtos;
using Data.Models;

namespace Contracts.Interfaces.Services;

public interface IRoleService
{
    public Task<HttpStatusCode> ChangeRoleName(Guid roleId, ChangeRoleNameDto changeRoleNameDto);
    public Task<(Role?, HttpStatusCode)> CreateRole(CreateRoleDto createRoleDto);
    public Task<HttpStatusCode> DeleteRole(Guid roleId);
    public Task<IEnumerable<Role>> GetAllRoles(bool includeDeleted, int page, int limit);
    public Task<IEnumerable<User>?> GetAllUsersWithRole(Guid roleId, bool includeDeleted, int page, int limit);
    public Task<Role?> GetRoleById(Guid roleId);
}