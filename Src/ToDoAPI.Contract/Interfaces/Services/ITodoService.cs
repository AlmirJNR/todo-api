using System.Net;
using Contracts.DTOs.TodoDtos;
using Data.Models;

namespace Contracts.Interfaces.Services;

public interface ITodoService
{
    public Task<HttpStatusCode> ChangeTodoCompleted(Guid todoId, ChangeTodoCompletedDto changeTodoCompletedDto);
    public Task<HttpStatusCode> ChangeTodoDescription(Guid todoId, ChangeTodoDescriptionDto changeTodoDescriptionDto);
    public Task<HttpStatusCode> ChangeTodoLimitDate(Guid todoId, ChangeTodoLimitDateDto changeTodoLimitDateDto);
    public Task<HttpStatusCode> ChangeTodoTitle(Guid todoId, ChangeTodoTitleDto changeTodoTitleDto);
    public Task<(Todo?, HttpStatusCode)> CreateTodo(CreateTodoDto createTodoDto);
    public Task<HttpStatusCode> DeleteTodo(Guid todoId);
    public Task<IEnumerable<Todo>> GetAllTodos(bool includeDeleted, int page, int limit);
    public Task<Todo?> GetTodoById(Guid todoId);
    public Task<HttpStatusCode> InsertTodoToGroup(Guid todoId, Guid groupId);
    public Task<HttpStatusCode> RemoveTodoFromGroup(Guid todoId);
}