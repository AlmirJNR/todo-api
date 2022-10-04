using Data.Models;

namespace Contracts.DTOs.TodoDtos;

public readonly record struct CreateTodoDto()
{
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public DateTime? LimitDate { get; init; } = null;

    public Todo ToModel() => new()
    {
        Title = Title,
        Description = Description,
        LimitDate = LimitDate
    };
}