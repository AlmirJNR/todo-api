using Data.Models;

namespace Contracts.DTOs.UserDtos;

public readonly record struct CreatedUserDto()
{
    public Guid AuthServerId { get; init; } = Guid.Empty;
    public DateTime? CreatedAt { get; init; } = null;
    public Guid? RoleId { get; init; } = Guid.Empty;

    public static CreatedUserDto FromModel(User userModel) => new()
    {
        AuthServerId = userModel.AuthServerId,
        CreatedAt = userModel.CreatedAt,
        RoleId = userModel.RoleId
    };
}