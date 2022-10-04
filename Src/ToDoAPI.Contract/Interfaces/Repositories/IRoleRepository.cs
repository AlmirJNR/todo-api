using System.Net;
using Data.Models;

namespace Contracts.Interfaces.Repositories;

public interface IRoleRepository
{
    public Task<(Role?, HttpStatusCode)> CreateRole(Role roleModel);
    public Task<HttpStatusCode> DeleteRole(Guid roleId);
    public Task<IEnumerable<Role>> GetAllRoles(bool includeDeleted, int page, int limit);
    public Task<IEnumerable<User>?> GetAllUsersWithRole(Guid roleId, bool includeDeleted, int page, int limit);
    public Task<Role?> GetRoleById(Guid roleId);
    public Task<HttpStatusCode> UpdateRole(Role roleModel);
}