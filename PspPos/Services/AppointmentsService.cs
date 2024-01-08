using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using PspPos.Commons;
using PspPos.Data;
using PspPos.Infrastructure;
using PspPos.Models;
using PspPos.Models.DTO.Requests;
using System;
using System.Linq.Expressions;

namespace PspPos.Services
{
    public class AppointmentsService: IAppointmentsService
    {
        private readonly ApplicationContext _context;
        private readonly IServiceService _serviceService;

        // this is not in AppliationContext because at this point we've strayed too far from the light
        private HashSet<Guid>? _availableAppointments = null;

        public AppointmentsService(ApplicationContext context, IServiceService serviceService)
        {
            _serviceService = serviceService;
            _context = context;
        }

        public async Task<bool> AppointmentsRelationshipsAreValid(Guid companyId, Guid serviceId)
        {
            return await _context.CheckIfCompanyExists(companyId) && await _serviceService.ExistsByPropertyAsync(x => x.Id == serviceId && x.companyId == companyId);
        }

        public async Task<Appointment> GetValidatedAppointment(AppointmentCreateRequest appointment, Guid companyId, Guid? appointmentId)
        {
            if(!await AppointmentsRelationshipsAreValid(companyId, appointment.ServiceId))
            {
                throw new NotFoundException("Relationships invalid");
            }

            if (!DateTime.TryParse(appointment.StartDate, out DateTime start) || !DateTime.TryParse(appointment.EndDate, out DateTime end))
            {
                throw new Exception("Failed to parse dates");
            }


            return new Appointment
            {
                ServiceId = appointment.ServiceId,
                CompanyId = companyId,
                Id = appointmentId ?? Guid.NewGuid(),
                StartDate = start,
                EndDate = end,
                OrderId = appointment.OrderId,
                StoreId = appointment.StoreId,
                WorkerId = appointment.WorkerId
            };
        }

        public async Task<HashSet<Guid>> GetAvailableAppointments()
        {
            if (_availableAppointments is null)
            {
                var companies = await _context.Appointments.ToListAsync();
                _availableAppointments = companies.Select(c => c.Id).ToHashSet()!;
            }

            return _availableAppointments;
        }

        public async Task<bool> CheckIfAppointmentExists(Guid id)
        {
            var appointments = await GetAvailableAppointments();
            if (appointments.Contains(id))
                return true;

            return false;
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
            (await GetAvailableAppointments()).Add(model.Id);
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
            (await GetAvailableAppointments()).Remove(model.Id);
            await _context.Instance.SaveChangesAsync();
        }

        // builds dynamic query based on parameters
        public async Task<IEnumerable<Appointment>> GetAllRequestedAppointments(Guid companyId, Guid? serviceId, string? lowerDateBoundary, string? higherDateBoundary )
        {
            var companiesAppointments = await GetAllByPropertyAsync(x => x.CompanyId == companyId);
           
            if (serviceId is not null) 
            {
                companiesAppointments = companiesAppointments.Where(x => x.ServiceId == serviceId);
            }

            if (DateTime.TryParse(lowerDateBoundary, out DateTime lowerBoundaryParsed) && DateTime.TryParse(higherDateBoundary, out DateTime higherBoundaryParsed))
            {
                companiesAppointments = companiesAppointments.Where(x => DateTime.Compare(lowerBoundaryParsed, x.StartDate) <= 0 && DateTime.Compare(higherBoundaryParsed, x.EndDate) >= 0);
            }

            return companiesAppointments.ToList();
        }
    }
}
