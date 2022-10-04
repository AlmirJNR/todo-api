using System.Net;
using Contracts.Interfaces.Repositories;
using Data.Context;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ToDoDatabaseContext _dbContext;
    private readonly DbSet<User> _userEntities;

    public UserRepository(ToDoDatabaseContext dbContext)
    {
        _dbContext = dbContext;
        _userEntities = dbContext.Users;
    }

    public async Task<(User?, HttpStatusCode)> CreateUser(User userModel)
    {
        var exists = await _userEntities.FirstOrDefaultAsync(user => user.AuthServerId == userModel.AuthServerId);

        if (exists is not null)
            return (null, HttpStatusCode.Conflict);

        var response = _userEntities.Add(userModel);
        var savedChanges = await _dbContext.SaveChangesAsync();

        return savedChanges != 1
            ? (null, HttpStatusCode.InternalServerError)
            : (response.Entity, HttpStatusCode.Created);
    }

    public async Task<HttpStatusCode> DeleteUser(Guid userAuthServerId)
    {
        var response = await _userEntities.FirstOrDefaultAsync(user => user.AuthServerId == userAuthServerId);

        if (response is null || response.DeletedAt is not null)
            return HttpStatusCode.NotFound;

        response.DeletedAt = DateTime.Now;

        _userEntities.Update(response);
        var savedChanges = await _dbContext.SaveChangesAsync();

        return savedChanges != 1
            ? HttpStatusCode.InternalServerError
            : HttpStatusCode.OK;
    }

    public async Task<IEnumerable<User>> GetAllUsers(bool includeDeleted, int page, int limit)
    {
        if (includeDeleted)
            return _userEntities
                .Skip((page - 1) * limit)
                .Take(limit);

        return _userEntities
            .Where(todo => todo.DeletedAt == null)
            .Skip((page - 1) * limit)
            .Take(limit);
    }

    public Task<User?> GetUserById(Guid userAuthServerId)
        => _userEntities.FirstOrDefaultAsync(user => user.AuthServerId == userAuthServerId);

    public async Task<HttpStatusCode> UpdateUser(User userModel)
    {
        var response = await _userEntities.FirstOrDefaultAsync(user => user.AuthServerId == userModel.AuthServerId);

        if (response is null)
            return HttpStatusCode.NotFound;

        if (userModel.RoleId is not null)
            response.RoleId = userModel.RoleId;

        _userEntities.Update(response);
        var savedChanges = await _dbContext.SaveChangesAsync();

        return savedChanges != 1
            ? HttpStatusCode.InternalServerError
            : HttpStatusCode.OK;
    }
}