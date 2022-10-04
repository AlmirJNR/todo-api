using Data.Models;

namespace Contracts.DTOs.GroupDtos;

public readonly record struct ChangeGroupNameDto(string Name)
{
    public Group ToModel() => new()
    {
        Name = Name
    };
}