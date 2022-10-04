using System.Net;
using Contracts.DTOs.UserDtos;
using Data.Models;

namespace Contracts.Interfaces.Services;

public interface IUserService
{
    public Task<HttpStatusCode> ChangeUserRole(Guid userAuthServerId, ChangeUserRoleDto changeUserRoleDto);
    public Task<(CreatedUserDto?, HttpStatusCode)> CreateUser(CreateUserDto createUserDto);
    public Task<HttpStatusCode> DeleteUser(Guid userAuthServerId);
    public Task<IEnumerable<ReducedUserDto>> GetAllUsers(bool includeDeleted, int page, int limit);
    public Task<User?> GetUserById(Guid userAuthServerId);
}