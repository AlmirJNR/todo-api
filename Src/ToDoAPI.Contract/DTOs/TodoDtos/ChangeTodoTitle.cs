using Data.Models;

namespace Contracts.DTOs.TodoDtos;

public readonly record struct ChangeTodoTitleDto()
{
    public string Title { get; init; } = string.Empty;

    public Todo ToModel() => new()
    {
        Title = Title
    };
}