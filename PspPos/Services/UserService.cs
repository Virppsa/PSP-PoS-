using AutoMapper;
using PspPos.Data;
using PspPos.Infrastructure;
using PspPos.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;

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

            if (isUserValid(user, companyID))
            {
                throw new KeyNotFoundException($"User with ID {userID} or CompanyID {companyID} not found.");
            }

            return user;
        }

        public async Task AddUser(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUser(Guid userID, Guid companyID, User updatedUser)
        {
            var existingUser = await _context.Users.FindAsync(userID);
            if (isUserValid(existingUser, companyID))
            {
                throw new KeyNotFoundException($"User with ID {userID} or CompanyID {companyID} not found.");
            }

            existingUser.Email = updatedUser.Email;
            existingUser.Phone = updatedUser.Phone;
            existingUser.Address = updatedUser.Address;
            existingUser.Role = updatedUser.Role;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteUser(Guid userID, Guid companyID)
        {
            var existingUser = await _context.Users.FindAsync(userID);
            if (isUserValid(existingUser, companyID))
            {
                throw new KeyNotFoundException($"User with ID {userID} or CompanyID {companyID} not found.");
            }

            _context.Users.Remove(existingUser);

            await _context.SaveChangesAsync();
        }

        private bool isUserValid(User user, Guid companyID)
        {
            return user != null && companyID == user.CompanyId;
        }
    }
}
