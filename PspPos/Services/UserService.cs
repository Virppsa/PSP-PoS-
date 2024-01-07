using AutoMapper;
using PspPos.Data;
using PspPos.Infrastructure;
using PspPos.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;
using PspPos.Models.DTO.Requests;

namespace PspPos.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationContext _context;
        private readonly IMapper _mapper;

        public UserService(ApplicationContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<User>> GetUsersByCompanyIdAsync(Guid companyId)
        {
            return await _context.Users
                                 .Where(u => u.CompanyId == companyId)
                                 .ToListAsync();
        }

        public async Task<User> GetUserByCompanyAndUserID(Guid companyID, Guid userID)
        {
            var user = await _context.Users.FindAsync(userID);

            if (!isUserValid(user, companyID))
            {
                throw new KeyNotFoundException($"User with ID {userID} or CompanyID {companyID} not found.");
            }

            return user;
        }

        public async Task<User> AddUser(UserPostModel user)
        {
            if (user is null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var createUser = _mapper.Map<User>(user);

            await _context.Users.AddAsync(createUser);
            await _context.SaveChangesAsync();

            return createUser;
        }

        public async Task<User> UpdateUser(Guid userID, Guid companyID, UserPostModel updatedUser)
        {
            var existingUser = await _context.Users.FindAsync(userID);
            if (!isUserValid(existingUser, companyID))
            {
                throw new KeyNotFoundException($"User with ID {userID} or CompanyID {companyID} not found.");
            }

            existingUser!.Email = updatedUser.Email;
            existingUser.Phone = updatedUser.Phone;
            existingUser.Address = updatedUser.Address;
            existingUser.Role = updatedUser.Role;
            existingUser.LoyaltyPoints = updatedUser.LoyaltyPoints;

            await _context.SaveChangesAsync();

            return existingUser;
        }

        public async Task<User> UpdateUserLoyalty(Guid userID, Guid companyID, UserLoyaltyUpdateRequest updatedUser)
        {
            var existingUser = await _context.Users.FindAsync(userID);
            if (!isUserValid(existingUser, companyID))
            {
                throw new KeyNotFoundException($"User with ID {userID} or CompanyID {companyID} not found.");
            }

            existingUser!.LoyaltyPoints = updatedUser.LoyaltyPoints;

            await _context.SaveChangesAsync();

            return existingUser;
        }

        public async Task DeleteUser(Guid userID, Guid companyID)
        {
            var existingUser = await _context.Users.FindAsync(userID);
            if (!isUserValid(existingUser, companyID))
            {
                throw new KeyNotFoundException($"User with ID {userID} or CompanyID {companyID} not found.");
            }

            _context.Users.Remove(existingUser);

            await _context.SaveChangesAsync();
        }

        private bool isUserValid(User? user, Guid companyID)
        {
            return user is not null && companyID == user.CompanyId;
        }
    }
}
