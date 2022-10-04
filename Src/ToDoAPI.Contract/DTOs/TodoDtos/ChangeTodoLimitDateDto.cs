using Data.Models;

namespace Contracts.DTOs.TodoDtos;

public readonly record struct ChangeTodoLimitDateDto()
{
    public DateTime? LimitDate { get; init; } = null;

    public Todo ToModel() => new()
    {
        LimitDate = LimitDate
    };
}