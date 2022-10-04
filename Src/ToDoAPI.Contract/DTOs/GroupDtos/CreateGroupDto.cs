using Data.Models;

namespace Contracts.DTOs.GroupDtos;

public readonly record struct CreateGroupDto()
{
    public string Name { get; init; } = null!;
    public Guid CreatedBy { get; init; } = Guid.Empty;
    public Guid Admin { get; init; } = Guid.Empty;

    public Group ToModel() => new()
    {
        Admin = Admin,
        CreatedBy = CreatedBy,
        Name = Name
    };
}