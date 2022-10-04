using System.Net;
using Data.Models;

namespace Contracts.Interfaces.Repositories;

public interface ITodoRepository
{
    public Task<(Todo?, HttpStatusCode)> CreateTodo(Todo todoModel);
    public Task<HttpStatusCode> DeleteTodo(Guid todoId);
    public Task<IEnumerable<Todo>> GetAllTodos(bool includeDeleted, int page, int limit);
    public Task<Todo?> GetTodoById(Guid todoId);
    public Task<HttpStatusCode> InsertTodoToGroup(Guid todoId, Guid groupId);
    public Task<HttpStatusCode> RemoveTodoFromGroup(Guid todoId);
    public Task<HttpStatusCode> UpdateTodo(Todo todoModel);
}