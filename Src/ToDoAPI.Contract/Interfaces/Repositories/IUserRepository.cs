using System.Net;
using Data.Models;

namespace Contracts.Interfaces.Repositories;

public interface IUserRepository
{
    public Task<(User?, HttpStatusCode)> CreateUser(User userModel);
    public Task<HttpStatusCode> DeleteUser(Guid userAuthServerId);
    public Task<IEnumerable<User>> GetAllUsers(bool includeDeleted, int page, int limit);
    public Task<User?> GetUserById(Guid userAuthServerId);
    public Task<HttpStatusCode> UpdateUser(User userModel);
}