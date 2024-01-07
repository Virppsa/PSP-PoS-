using PspPos.Models;
using System.Linq.Expressions;

namespace PspPos.Infrastructure
{
    public interface IAppointmentsService
    {
        public Task<IEnumerable<Appointment>> GetAllAsync();

        public Task<IEnumerable<Appointment>> GetAllByPropertyAsync(Expression<Func<Appointment, bool>> predicate);

        public Task<Appointment?> GetByPropertyAsync(Expression<Func<Appointment, bool>> predicate);

        public Task<bool> ExistsByPropertyAsync(Expression<Func<Appointment, bool>> predicate);

        public Task InsertAsync(Appointment model);

        public Task UpdateAsync(Appointment model);

        public Task DeleteAsync(Appointment model);

        public Task<IEnumerable<Appointment>> GetAllRequestedAppointments(Guid companyId, Guid? serviceId, DateTime? lowerDateBoundary, DateTime? higherDateBoundary);
    }
}
