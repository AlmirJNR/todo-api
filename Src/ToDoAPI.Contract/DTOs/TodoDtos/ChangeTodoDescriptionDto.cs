using Data.Models;

namespace Contracts.DTOs.TodoDtos;

public readonly record struct ChangeTodoDescriptionDto()
{
    public string Description { get; init; } = string.Empty;

    public Todo ToModel() => new()
    {
        Description = Description
    };
}