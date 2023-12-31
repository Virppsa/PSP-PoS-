﻿using Microsoft.EntityFrameworkCore;
using PspPos.Data;
using PspPos.Models;
using System.Linq.Expressions;

namespace PspPos.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly ApplicationContext _context;

        public AppointmentRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Appointment>> GetAllAsync()
        {
            return await _context.Appointments.ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetAllByPropertyAsync(Expression<Func<Appointment, bool>> predicate)
        {
            return await _context.Appointments.Where(predicate).ToListAsync();
        }

        public Task<Appointment?> GetByPropertyAsync(Expression<Func<Appointment, bool>> predicate)
        {
            return _context.Appointments.FirstOrDefaultAsync(predicate);
        }

        public Task<bool> ExistsByPropertyAsync(Expression<Func<Appointment, bool>> predicate)
        {
            return _context.Appointments.AnyAsync(predicate);
        }

        public async Task InsertAsync(Appointment model)
        {
            await _context.Appointments.AddAsync(model);
            await _context.Instance.SaveChangesAsync();
        }

        public async Task UpdateAsync(Appointment model)
        {
            _context.Appointments.Update(model);
            await _context.Instance.SaveChangesAsync();
        }

        public async Task DeleteAsync(Appointment model)
        {
            _context.Appointments.Remove(model);
            await _context.Instance.SaveChangesAsync();
        }
    }
}
