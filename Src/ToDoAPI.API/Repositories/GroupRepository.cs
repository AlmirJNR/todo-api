using System.Net;
using Contracts.Interfaces.Repositories;
using Data.Context;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories;

public class GroupRepository : IGroupRepository
{
    private readonly ToDoDatabaseContext _dbContext;
    private readonly DbSet<Group> _groupEntities;

    public GroupRepository(ToDoDatabaseContext dbContext)
    {
        _dbContext = dbContext;
        _groupEntities = dbContext.Groups;
    }

    public async Task<(Group?, HttpStatusCode)> CreateGroup(Group groupModel)
    {
        var response = await _groupEntities.AddAsync(groupModel);
        var savedChanges = await _dbContext.SaveChangesAsync();

        return savedChanges != 1
            ? (null, HttpStatusCode.InternalServerError)
            : (response.Entity, HttpStatusCode.Created);
    }

    public async Task<HttpStatusCode> DeleteGroup(Guid groupId)
    {
        var response = await _groupEntities.FirstOrDefaultAsync(group => group.Id == groupId);

        if (response is null)
            return HttpStatusCode.NotFound;

        response.DeletedAt = DateTime.Now;

        foreach (var user in response.Users)
        {
            user.Groups.Remove(response);
        }

        _groupEntities.Update(response);
        var savedChanges = await _dbContext.SaveChangesAsync();

        return savedChanges != 1
            ? HttpStatusCode.InternalServerError
            : HttpStatusCode.OK;
    }

    public async Task<Group?> GetGroupById(Guid groupId)
        => await _groupEntities.FirstOrDefaultAsync(group => group.Id == groupId);

    public async Task<HttpStatusCode> InsertUserIntoGroup(User userModel, Guid groupId)
    {
        var response = await _groupEntities.FirstOrDefaultAsync(group => group.Id == groupId);

        if (response is null)
            return HttpStatusCode.NotFound;

        response.Users.Add(userModel);

        _groupEntities.Update(response);
        var savedChanges = await _dbContext.SaveChangesAsync();

        return savedChanges != 1
            ? HttpStatusCode.InternalServerError
            : HttpStatusCode.OK;
    }

    public async Task<HttpStatusCode> RemoveUserFromGroup(Guid userAuthServerId, Guid groupId)
    {
        var response = await _groupEntities.FirstOrDefaultAsync(group => group.Id == groupId);

        var userToRemove = response?.Users.FirstOrDefault(user => user.AuthServerId == userAuthServerId);

        if (response is null || userToRemove is null)
            return HttpStatusCode.NotFound;

        var removedWithSuccess = response.Users.Remove(userToRemove);
        if (!removedWithSuccess)
            return HttpStatusCode.InternalServerError;

        _groupEntities.Update(response);
        var savedChanges = await _dbContext.SaveChangesAsync();

        return savedChanges != 1
            ? HttpStatusCode.InternalServerError
            : HttpStatusCode.OK;
    }

    public async Task<HttpStatusCode> UpdateGroup(Group groupModel)
    {
        var response = await _groupEntities.FirstOrDefaultAsync(group => group.Id == groupModel.Id);

        if (response is null)
            return HttpStatusCode.NotFound;

        if (!string.IsNullOrWhiteSpace(groupModel.Name))
            response.Name = groupModel.Name;

        _groupEntities.Update(response);
        var savedChanges = await _dbContext.SaveChangesAsync();

        return savedChanges != 1
            ? HttpStatusCode.InternalServerError
            : HttpStatusCode.OK;
    }
}