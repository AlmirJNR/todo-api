using System.Net;
using Contracts.DTOs.UserDtos;
using Contracts.Interfaces.Repositories;
using Contracts.Interfaces.Services;
using Data.Models;

namespace API.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;

    public UserService(IUserRepository userRepository, IRoleRepository roleRepository)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
    }

    public async Task<HttpStatusCode> ChangeUserRole(Guid userAuthServerId, ChangeUserRoleDto changeUserRoleDto)
    {
        var roleExists = await _roleRepository.GetRoleById(changeUserRoleDto.RoleId);

        if (roleExists is null)
            return HttpStatusCode.NotFound;

        var userModel = changeUserRoleDto.ToModel();
        userModel.AuthServerId = userAuthServerId;

        return await _userRepository.UpdateUser(userModel);
    }

    public async Task<(CreatedUserDto?, HttpStatusCode)> CreateUser(CreateUserDto createUserDto)
    {
        var (userModel, httpStatusCode) = await _userRepository.CreateUser(createUserDto.ToModel());

        if (userModel is null)
            return (null, HttpStatusCode.NotFound);

        return (CreatedUserDto.FromModel(userModel), httpStatusCode);
    }

    public Task<HttpStatusCode> DeleteUser(Guid userAuthServerId)
        => _userRepository.DeleteUser(userAuthServerId);

    public async Task<IEnumerable<ReducedUserDto>> GetAllUsers(bool includeDeleted, int page, int limit)
    {
        var allUsers = await _userRepository.GetAllUsers(includeDeleted, page, limit);
        return allUsers.Select(ReducedUserDto.FromModel);
    }

    public Task<User?> GetUserById(Guid userAuthServerId)
        => _userRepository.GetUserById(userAuthServerId);
}