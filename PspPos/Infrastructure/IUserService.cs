using PspPos.Models;

namespace PspPos.Infrastructure
{
    public interface IUserService
    {

        public Task<List<User>> GetUsersByCompanyIdAsync(Guid companyId);

        public Task<User> GetUserByCompanyAndUserID(Guid companyID, Guid userID);

        public Task<User> AddUser(UserPostModel user);

        public Task<User> UpdateUser(Guid userID, Guid companyID, UserPostModel updatedUser);

        public Task DeleteUser(Guid userID, Guid companyID);
    }
}
