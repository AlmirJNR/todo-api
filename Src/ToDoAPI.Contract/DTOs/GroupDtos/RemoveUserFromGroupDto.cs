namespace Contracts.DTOs.GroupDtos;

public readonly record struct RemoveUserFromGroupDto()
{
    public Guid UserId { get; init; } = Guid.Empty;
}