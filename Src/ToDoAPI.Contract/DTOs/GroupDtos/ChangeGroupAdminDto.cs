using Data.Models;

namespace Contracts.DTOs.GroupDtos;

public readonly record struct ChangeGroupAdminDto()
{
    public Guid Admin { get; init; } = Guid.Empty;

    public Group ToModel() => new()
    {
        Admin = Admin
    };
}