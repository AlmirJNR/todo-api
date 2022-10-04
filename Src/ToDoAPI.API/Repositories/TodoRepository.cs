using System.Net;
using Contracts.Interfaces.Repositories;
using Data.Context;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories;

public class TodoRepository : ITodoRepository
{
    private readonly ToDoDatabaseContext _dbContext;
    private readonly DbSet<Todo> _todoEntities;

    public TodoRepository(ToDoDatabaseContext dbContext)
    {
        _dbContext = dbContext;
        _todoEntities = dbContext.Todos;
    }

    public async Task<(Todo?, HttpStatusCode)> CreateTodo(Todo todoModel)
    {
        var response = await _todoEntities.AddAsync(todoModel);
        var savedChanges = await _dbContext.SaveChangesAsync();

        return savedChanges != 1
            ? (null, HttpStatusCode.InternalServerError)
            : (response.Entity, HttpStatusCode.Created);
    }

    public async Task<HttpStatusCode> DeleteTodo(Guid todoId)
    {
        var response = await _todoEntities.FirstOrDefaultAsync(todo => todo.Id == todoId);

        if (response is null)
            return HttpStatusCode.NotFound;

        response.DeletedAt = DateTime.Now;

        _todoEntities.Update(response);
        var savedChanges = await _dbContext.SaveChangesAsync();

        return savedChanges != 1
            ? HttpStatusCode.InternalServerError
            : HttpStatusCode.OK;
    }

    public async Task<IEnumerable<Todo>> GetAllTodos(bool includeDeleted, int page, int limit)
    {
        if (includeDeleted)
            return _todoEntities
                .Skip((page - 1) * limit)
                .Take(limit);

        return _todoEntities
            .Where(todo => todo.DeletedAt == null)
            .Skip((page - 1) * limit)
            .Take(limit);
    }

    public Task<Todo?> GetTodoById(Guid todoId)
        => _todoEntities.FirstOrDefaultAsync(todo => todo.Id == todoId);

    public async Task<HttpStatusCode> InsertTodoToGroup(Guid todoId, Guid groupId)
    {
        var response = await _todoEntities.FirstOrDefaultAsync(todo => todo.Id == todoId);

        if (response is null)
            return HttpStatusCode.NotFound;

        if (response.InGroup is not null)
            return HttpStatusCode.Conflict;

        response.InGroup = groupId;

        _todoEntities.Update(response);
        var savedChanges = await _dbContext.SaveChangesAsync();

        return savedChanges != 1
            ? HttpStatusCode.InternalServerError
            : HttpStatusCode.OK;
    }

    public async Task<HttpStatusCode> RemoveTodoFromGroup(Guid todoId)
    {
        var response = await _todoEntities.FirstOrDefaultAsync(todo => todo.Id == todoId);

        if (response is null)
            return HttpStatusCode.NotFound;

        response.InGroup = null;

        _todoEntities.Update(response);
        var savedChanges = await _dbContext.SaveChangesAsync();

        return savedChanges != 1
            ? HttpStatusCode.InternalServerError
            : HttpStatusCode.OK;
    }

    public async Task<HttpStatusCode> UpdateTodo(Todo todoModel)
    {
        var response = await _todoEntities.FirstOrDefaultAsync(todo => todo.Id == todoModel.Id);

        if (response is null)
            return HttpStatusCode.NotFound;

        if (!string.IsNullOrWhiteSpace(todoModel.Title))
            response.Title = todoModel.Title;

        if (!string.IsNullOrWhiteSpace(todoModel.Title))
            response.Description = todoModel.Description;

        if (todoModel.Completed is not null)
            response.Completed = todoModel.Completed;

        if (todoModel.LimitDate is not null)
            response.LimitDate = todoModel.LimitDate;

        _todoEntities.Update(response);
        var savedChanges = await _dbContext.SaveChangesAsync();

        return savedChanges != 1
            ? HttpStatusCode.InternalServerError
            : HttpStatusCode.OK;
    }
}