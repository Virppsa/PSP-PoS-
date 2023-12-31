﻿using PspPos.Models;
using System.Linq.Expressions;

namespace PspPos.Repositories
{
    public interface IAppointmentRepository
    {
        public Task<IEnumerable<Appointment>> GetAllAsync();

        public Task<IEnumerable<Appointment>> GetAllByPropertyAsync(Expression<Func<Appointment, bool>> predicate);

        public Task<Appointment?> GetByPropertyAsync(Expression<Func<Appointment, bool>> predicate);

        public Task<bool> ExistsByPropertyAsync(Expression<Func<Appointment, bool>> predicate);

        public Task InsertAsync(Appointment model);

        public Task UpdateAsync(Appointment model);

        public Task DeleteAsync(Appointment model);
    }
}
