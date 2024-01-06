using PspPos.Models;
using System.Linq.Expressions;

namespace PspPos.Repositories
{
     public interface IServiceRepository
     {
         public Task<IEnumerable<Service>> GetAllAsync();

        public Task<IEnumerable<Service>> GetAllByPropertyAsync(Expression<Func<Service, bool>> predicate);

        public Task<Service?> GetByPropertyAsync(Expression<Func<Service, bool>> predicate);

         public Task<bool> ExistsByPropertyAsync(Expression<Func<Service, bool>> predicate);

         public Task InsertAsync(Service model);

         public Task UpdateAsync(Service model);

         public Task DeleteAsync(Service model);
     }
}
