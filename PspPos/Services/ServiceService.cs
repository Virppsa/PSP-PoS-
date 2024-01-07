using Microsoft.EntityFrameworkCore;
using PspPos.Data;
using PspPos.Infrastructure;
using PspPos.Models;
using System.Linq.Expressions;

namespace PspPos.Services
{
    public class ServiceService : IServiceService
    {
        private readonly ApplicationContext _context;

        public ServiceService(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Service>> GetAllAsync()
        {
            return await _context.Services.ToListAsync();
        }

        public async Task<IEnumerable<Service>> GetAllByPropertyAsync(Expression<Func<Service, bool>> predicate)
        {
            return await _context.Services.Where(predicate).ToListAsync();
        }

        public Task<Service?> GetByPropertyAsync(Expression<Func<Service, bool>> predicate)
        {
            return _context.Services.FirstOrDefaultAsync(predicate);
        }

        public Task<bool> ExistsByPropertyAsync(Expression<Func<Service, bool>> predicate)
        {
            return _context.Services.AnyAsync(predicate);
        }

        public async Task InsertAsync(Service model)
        {
            await _context.Services.AddAsync(model);
            await _context.Instance.SaveChangesAsync();
        }

        public async Task UpdateAsync(Service model)
        {
            _context.Services.Update(model);
            await _context.Instance.SaveChangesAsync();
        }

        public async Task DeleteAsync(Service model)
        {
            _context.Services.Remove(model);
            await _context.Instance.SaveChangesAsync();
        }
    }
}
