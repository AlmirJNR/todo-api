using System.Net;
using Contracts.DTOs.RoleDtos;
using Contracts.Interfaces.Repositories;
using Contracts.Interfaces.Services;
using Data.Models;

namespace API.Services;

public class RoleService : IRoleService
{
    private readonly IRoleRepository _roleRepository;

    public RoleService(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public Task<HttpStatusCode> ChangeRoleName(Guid roleId, ChangeRoleNameDto changeRoleNameDto)
    {
        var roleModel = changeRoleNameDto.ToModel();
        roleModel.Id = roleId;

        return _roleRepository.UpdateRole(roleModel);
    }

    public Task<(Role?, HttpStatusCode)> CreateRole(CreateRoleDto createRoleDto)
        => _roleRepository.CreateRole(createRoleDto.ToModel());

    public Task<HttpStatusCode> DeleteRole(Guid roleId)
        => _roleRepository.DeleteRole(roleId);

    public Task<IEnumerable<Role>> GetAllRoles(bool includeDeleted, int page, int limit)
        => _roleRepository.GetAllRoles(includeDeleted, page, limit);

    public Task<IEnumerable<User>?> GetAllUsersWithRole(Guid roleId, bool includeDeleted, int page, int limit)
        => _roleRepository.GetAllUsersWithRole(roleId, includeDeleted, page, limit);

    public Task<Role?> GetRoleById(Guid roleId)
        => _roleRepository.GetRoleById(roleId);
}