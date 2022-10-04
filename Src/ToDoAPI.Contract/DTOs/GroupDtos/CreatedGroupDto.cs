using Data.Models;

namespace Contracts.DTOs.GroupDtos;

public readonly record struct CreatedGroupDto()
{
    public Guid Id { get; init; } = Guid.Empty;
    public string Name { get; init; } = null!;
    public DateTime? CreatedAt { get; init; } = null;
    public Guid CreatedBy { get; init; } = Guid.Empty;
    public Guid Admin { get; init; } = Guid.Empty;

    public static CreatedGroupDto FromModel(Group groupModel) => new()
    {
        Id = groupModel.Id,
        Name = groupModel.Name,
        CreatedAt = groupModel.CreatedAt,
        CreatedBy = groupModel.CreatedBy,
        Admin = groupModel.Admin
    };
}