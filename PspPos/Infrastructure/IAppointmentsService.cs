using PspPos.Models;
using PspPos.Models.DTO.Requests;
using System.Linq.Expressions;
using static PspPos.Models.DTO.Requests.AppointmentCreateRequest;

namespace PspPos.Infrastructure
{
    public interface IAppointmentsService
    {
        public Task<bool> CheckIfAppointmentExists(Guid id);
        public Task<IEnumerable<Appointment>> GetAllAsync();

        public Task<IEnumerable<Appointment>> GetAllByPropertyAsync(Expression<Func<Appointment, bool>> predicate);

        public Task<Appointment?> GetByPropertyAsync(Expression<Func<Appointment, bool>> predicate);

        public Task<bool> ExistsByPropertyAsync(Expression<Func<Appointment, bool>> predicate);

        public Task InsertAsync(Appointment model);

        public Task UpdateAsync(Appointment model);

        public Task DeleteAsync(Appointment model);

        public Task<IEnumerable<Appointment>> GetAllRequestedAppointments(Guid companyId, Guid? serviceId, string? lowerDateBoundary, string? higherDateBoundary);

        public Task<bool> AppointmentsRelationshipsAreValid(Guid companyId, Guid serviceId);

        public Task<Appointment> GetValidatedAppointment(AppointmentCreateRequest appointment, Guid companyId, Guid? appointmentId);

        public Task<Appointment> GetValidatedUpdatedAppointment(AppointmentUpdateRequest appointment, Guid companyId, Guid? appointmentId);
    }
}
