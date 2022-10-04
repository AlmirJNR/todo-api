using System.Net;
using Contracts.DTOs.TodoDtos;
using Contracts.Interfaces.Repositories;
using Contracts.Interfaces.Services;
using Data.Models;

namespace API.Services;

public class TodoService : ITodoService
{
    private readonly ITodoRepository _todoRepository;

    public TodoService(ITodoRepository todoRepository)
    {
        _todoRepository = todoRepository;
    }

    public Task<HttpStatusCode> ChangeTodoCompleted(Guid todoId, ChangeTodoCompletedDto changeTodoCompletedDto)
    {
        var todoModel = changeTodoCompletedDto.ToModel();
        todoModel.Id = todoId;

        return _todoRepository.UpdateTodo(todoModel);
    }

    public Task<HttpStatusCode> ChangeTodoDescription(Guid todoId, ChangeTodoDescriptionDto changeTodoDescriptionDto)
    {
        var todoModel = changeTodoDescriptionDto.ToModel();
        todoModel.Id = todoId;

        return _todoRepository.UpdateTodo(todoModel);
    }

    public Task<HttpStatusCode> ChangeTodoLimitDate(Guid todoId, ChangeTodoLimitDateDto changeTodoLimitDateDto)
    {
        var todoModel = changeTodoLimitDateDto.ToModel();
        todoModel.Id = todoId;

        return _todoRepository.UpdateTodo(todoModel);
    }

    public Task<HttpStatusCode> ChangeTodoTitle(Guid todoId, ChangeTodoTitleDto changeTodoTitleDto)
    {
        var todoModel = changeTodoTitleDto.ToModel();
        todoModel.Id = todoId;

        return _todoRepository.UpdateTodo(todoModel);
    }

    public Task<(Todo?, HttpStatusCode)> CreateTodo(CreateTodoDto createTodoDto)
        => _todoRepository.CreateTodo(createTodoDto.ToModel());

    public Task<HttpStatusCode> DeleteTodo(Guid todoId)
        => _todoRepository.DeleteTodo(todoId);

    public Task<IEnumerable<Todo>> GetAllTodos(bool includeDeleted, int page, int limit)
        => _todoRepository.GetAllTodos(includeDeleted, page, limit);

    public Task<Todo?> GetTodoById(Guid todoId)
        => _todoRepository.GetTodoById(todoId);

    public Task<HttpStatusCode> InsertTodoToGroup(Guid todoId, Guid groupId)
        => _todoRepository.InsertTodoToGroup(todoId, groupId);

    public Task<HttpStatusCode> RemoveTodoFromGroup(Guid todoId)
        => _todoRepository.RemoveTodoFromGroup(todoId);
}