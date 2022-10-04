using Data.Models;

namespace Contracts.DTOs.UserDtos;

public readonly record struct CreateUserDto()
{
    public Guid AuthServerId { get; init; } = Guid.Empty;
    public Guid? RoleId { get; init; } = Guid.Empty;

    public User ToModel() => new()
    {
        AuthServerId = AuthServerId,
        RoleId = RoleId
    };
}