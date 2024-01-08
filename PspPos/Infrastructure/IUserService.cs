using PspPos.Models;
using PspPos.Models.DTO.Requests;

namespace PspPos.Infrastructure
{
    public interface IUserService
    {

        public Task<List<User>> GetUsersByCompanyIdAsync(Guid companyId);

        public Task<User> GetUserByCompanyAndUserID(Guid companyID, Guid userID);

        public Task<User> AddUser(UserPostModel user, Guid companyId);

        public Task<User> UpdateUser(Guid userID, Guid companyID, UserPostModel updatedUser);

        public Task<User> UpdateUserLoyalty(Guid userID, Guid companyID, UserLoyaltyUpdateRequest updatedUser);

        public Task DeleteUser(Guid userID, Guid companyID);
    }
}
