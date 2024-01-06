using PspPos.Models;

namespace PspPos.Infrastructure
{
    public interface IUserService
    {

        public Task<List<User>> GetUsersByCompanyIdAsync(Guid companyId);

        public Task<User> GetUserByCompanyAndUserID(Guid companyID, Guid userID);

        public Task AddUser(User user);

        public Task UpdateUser(Guid userID, Guid companyID, User updatedUser);

        public Task DeleteUser(Guid userID, Guid companyID);
    }
}
