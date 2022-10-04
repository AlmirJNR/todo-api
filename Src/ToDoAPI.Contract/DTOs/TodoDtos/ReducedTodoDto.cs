using Data.Models;

namespace Contracts.DTOs.TodoDtos;

public readonly record struct ReducedTodoDto()
{
    public Guid Id { get; init; } = Guid.Empty;
    public string Title { get; init; } = string.Empty;
    public bool? Completed { get; init; } = null;
    public DateTime? LimitDate { get; init; } = null;
    public DateTime? CreatedAt { get; init; } = null;

    public static ReducedTodoDto FromModel(Todo todoModel) => new()
    {
        Id = todoModel.Id,
        Title = todoModel.Title,
        Completed = todoModel.Completed,
        LimitDate = todoModel.LimitDate,
        CreatedAt = todoModel.CreatedAt
    };
}