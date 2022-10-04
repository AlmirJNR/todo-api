using System.Net;
using Contracts.Interfaces.Repositories;
using Data.Context;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly ToDoDatabaseContext _dbContext;
    private readonly DbSet<Role> _roleEntities;

    public RoleRepository(ToDoDatabaseContext dbContext)
    {
        _dbContext = dbContext;
        _roleEntities = dbContext.Roles;
    }

    public async Task<(Role?, HttpStatusCode)> CreateRole(Role roleModel)
    {
        var exists = await _roleEntities.FirstOrDefaultAsync(role => string.Equals(role.Name, roleModel.Name));

        if (exists is not null)
            return (null, HttpStatusCode.Conflict);

        var response = await _roleEntities.AddAsync(roleModel);
        var savedChanges = await _dbContext.SaveChangesAsync();

        return savedChanges != 1
            ? (null, HttpStatusCode.InternalServerError)
            : (response.Entity, HttpStatusCode.OK);
    }

    public async Task<HttpStatusCode> DeleteRole(Guid roleId)
    {
        var response = await _roleEntities.FirstOrDefaultAsync(role => role.Id == roleId);

        if (response is null)
            return HttpStatusCode.NotFound;

        response.DeletedAt = DateTime.Now;

        _roleEntities.Update(response);
        var savedChanges = await _dbContext.SaveChangesAsync();

        return savedChanges != 1
            ? HttpStatusCode.InternalServerError
            : HttpStatusCode.OK;
    }

    public async Task<IEnumerable<Role>> GetAllRoles(bool includeDeleted, int page, int limit)
    {
        if (includeDeleted)
            return _roleEntities
                .Skip((page - 1) * limit)
                .Take(limit);

        return _roleEntities
            .Where(role => role.DeletedAt == null)
            .Skip((page - 1) * limit)
            .Take(limit);
    }

    public async Task<IEnumerable<User>?> GetAllUsersWithRole(Guid roleId, bool includeDeleted, int page, int limit)
    {
        var response = await _roleEntities.FirstOrDefaultAsync(role => role.Id == roleId);

        if (response is null)
            return null;

        if (includeDeleted)
            return response.Users
                .Skip((page - 1) * limit)
                .Take(limit);

        return response.Users
            .Where(role => role.DeletedAt == null)
            .Skip((page - 1) * limit)
            .Take(limit);
    }

    public async Task<Role?> GetRoleById(Guid roleId)
        => await _roleEntities.FirstOrDefaultAsync(role => role.Id == roleId);


    public async Task<HttpStatusCode> UpdateRole(Role roleModel)
    {
        var response = await _roleEntities.FirstOrDefaultAsync(role => role.Id == roleModel.Id);

        if (response is null)
            return HttpStatusCode.NotFound;

        if (!string.IsNullOrEmpty(roleModel.Name))
            response.Name = roleModel.Name;

        _roleEntities.Update(response);
        var savedChanges = await _dbContext.SaveChangesAsync();

        return savedChanges != 1
            ? HttpStatusCode.InternalServerError
            : HttpStatusCode.OK;
    }
}