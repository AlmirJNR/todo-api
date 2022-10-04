using Data.Models;

namespace Contracts.DTOs.UserDtos;

public readonly record struct ChangeUserRoleDto()
{
    public Guid RoleId { get; init; } = Guid.Empty;

    public User ToModel() => new()
    {
        RoleId = RoleId
    };
}