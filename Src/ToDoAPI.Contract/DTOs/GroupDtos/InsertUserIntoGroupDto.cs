namespace Contracts.DTOs.GroupDtos;

public readonly record struct InsertUserIntoGroupDto()
{
    public Guid UserId { get; init; } = Guid.Empty;
}