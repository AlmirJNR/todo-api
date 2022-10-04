using Data.Models;

namespace Contracts.DTOs.TodoDtos;

public readonly record struct ChangeTodoCompletedDto()
{
    public bool? Completed { get; init; } = null;

    public Todo ToModel() => new()
    {
        Completed = Completed
    };
}