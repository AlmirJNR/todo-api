using Data.Models;

namespace Contracts.DTOs.RoleDtos;

public readonly record struct ChangeRoleNameDto()
{
    public string Name { get; init; } = string.Empty;

    public Role ToModel() => new()
    {
        Name = Name
    };
};