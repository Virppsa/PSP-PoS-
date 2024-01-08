using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Newtonsoft.Json;
using PspPos.Commons;
using PspPos.Data;
using PspPos.Infrastructure;
using PspPos.Models;
using PspPos.Models.DTO.Requests;
using PspPos.Repositories;
using PspPos.Services;

namespace PspPos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private readonly IServiceService _serviceService;
        //private readonly IAppointmentService _appointmentsRepository;
        private readonly IAppointmentsService _appointmentsService;
        private readonly ApplicationContext _context;

        public ServiceController(IServiceService serviceService, ApplicationContext context, IAppointmentsService appointmentsService)
        {
            _serviceService = serviceService;
            _appointmentsService = appointmentsService;
            _context = context;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("/cinematic/{companyId}/services")]
        public async Task<ActionResult<IEnumerable<Service>>> GetAllServices(Guid companyId)
        {
            IEnumerable<Service> services;

            if (!await _context.CheckIfCompanyExists(companyId))
            {
                return StatusCode(StatusCodes.Status404NotFound, "Provided Company does not exist");
            }

            try
            {
                services = await _serviceService.GetAllByPropertyAsync(x => x.companyId == companyId);
            } 
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return Ok(services);

        }
        
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPost("/cinematic/{companyId}/services")]
        public async Task<ActionResult<Service>> CreateService(ServiceCreateRequest body, Guid companyId)
        {
            if (!await _context.CheckIfCompanyExists(companyId))
            {
                return StatusCode(StatusCodes.Status404NotFound, "Provided Company does not exist");
            }

            var service = new Service { Id = Guid.NewGuid(), companyId = companyId, Description = body.Description, Price = body.Price, Name = body.Name };
            // calculate tax and add also
            await _serviceService.InsertAsync(service);

            return Ok(service);

        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("/cinematic/{companyId}/services/{serviceId}")]
        public async Task<ActionResult<IEnumerable<Service>>> GetService(Guid companyId, Guid serviceId)
        {
           Service? service;

           if (!await _context.CheckIfCompanyExists(companyId))
           {
               return StatusCode(StatusCodes.Status404NotFound, "Provided Company does not exist");
           }

           try
           {
               service = await _serviceService.GetByPropertyAsync(x => x.companyId == companyId && x.Id == serviceId);
           }
           catch 
           {
               return StatusCode(StatusCodes.Status401Unauthorized);
           }

           if (service == null)
           {
               return StatusCode(StatusCodes.Status404NotFound);
           }

           return Ok(service);

        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPut("/cinematic/{companyId}/services/{serviceId}")]
        public async Task<ActionResult<Service>> EditService(ServiceCreateRequest body, Guid companyId, Guid serviceId)
        {
            if (!await _context.CheckIfCompanyExists(companyId))
            {
                return StatusCode(StatusCodes.Status404NotFound, "Provided Company does not exist");
            }

            var service = new Service { Id = serviceId, companyId = companyId, Description = body.Description, Price = body.Price, Name = body.Name, Tax = body.Price * TaxSystem.TaxMultiplier };
            // calculate tax and add also

            try 
            {
                await _serviceService.UpdateAsync(service);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to Update Service");
            }

            return Ok(service);
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("/cinematic/{companyId}/services/{serviceId}")]
        public async Task<ActionResult> Delete(Guid companyId, Guid serviceId)
        {
            if (!await _context.CheckIfCompanyExists(companyId))
            {
                return StatusCode(StatusCodes.Status404NotFound, "Provided Company does not exist");
            }

            Service? service;

            try
            {
                service = await _serviceService.GetByPropertyAsync(x => x.Id == serviceId);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            if(service == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }

            try
            {
                await _serviceService.DeleteAsync(service);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return StatusCode(StatusCodes.Status204NoContent);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPut("/cinematic/{companyId}/services/{serviceId}/discount")]
        public async Task<ActionResult<ServiceDiscount>> AddDiscountToService(Guid companyId, Guid serviceId)
        {
            if (!await _context.CheckIfCompanyExists(companyId))
            {
                return StatusCode(StatusCodes.Status404NotFound, "Provided Company does not exist");
            }

            Service? service = await _serviceService.GetByPropertyAsync(x => x.Id == serviceId);
            if(service == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, "Provided Service does not exist");
            }

            // should be moved to service
            var discount = TaxSystem.CreateDefaultServiceDiscount(serviceId);
            var serviceWithDiscount = new Service { Id = serviceId, companyId = companyId, Description = service.Description, Price = service.Price, Name = service.Name, Tax = service.Tax, SerializedDiscount = JsonConvert.SerializeObject(discount) };

            try
            {
               await _serviceService.UpdateAsync(service);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to Apply Discount to a Service");
            }

            return Ok(discount);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("/cinematic/{companyId}/appointments/get")]
        public async Task<ActionResult<IEnumerable<GetAppointmentsRequest>>> GetAllAppointments([FromBody] GetAppointmentsRequest request, Guid companyId)
        {
            IEnumerable<Appointment> appointments;

            if (!await _context.CheckIfCompanyExists(companyId))
            {
                return StatusCode(StatusCodes.Status404NotFound, "Provided Company does not exist");
            }

            try
            {
                appointments = await _appointmentsService.GetAllRequestedAppointments(companyId, request.ServiceId, request.LowerDateBoundary, request.HigherDateBoundary);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return Ok(appointments);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("/cinematic/{companyId}/appointments")]
        public async Task<ActionResult<Appointment>> CreateAppointment([FromBody] AppointmentCreateRequest body, Guid companyId)
        {
            if (!await _context.CheckIfCompanyExists(companyId))
            {
                return StatusCode(StatusCodes.Status404NotFound, "Provided Company does not exist");
            }

            Appointment? appointment;

           try
            {
                appointment = await _appointmentsService.GetValidatedAppointment(body, companyId, null);
                await _appointmentsService.InsertAsync(appointment);
            }
            catch (NotFoundException)
            {
                return NotFound("The service does not exist");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.InnerException?.ToString() ?? ex.Message);
            }

            return Ok(appointment);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("/cinematic/{companyId}/appointments/{appointmentId}")]
        public async Task<ActionResult<IEnumerable<Service>>> GetAppointment(Guid companyId, Guid appointmentId)
        {
            Appointment? appointment;

            if (!await _context.CheckIfCompanyExists(companyId))
            {
                return StatusCode(StatusCodes.Status404NotFound, "Provided Company does not exist");
            }

            try
            {
                appointment = await _appointmentsService.GetByPropertyAsync(x => x.CompanyId == companyId && x.Id == appointmentId);
            }
            catch
            {
                return StatusCode(StatusCodes.Status401Unauthorized);
            }

            if (appointment is null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }

            return Ok(appointment);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPut("/cinematic/{companyId}/appointments/{appointmentId}")]
        public async Task<ActionResult<Appointment>> EditAppointment([FromBody] AppointmentCreateRequest body, Guid companyId, Guid appointmentId)
        {
            if (!await _context.CheckIfCompanyExists(companyId))
            {
                return StatusCode(StatusCodes.Status404NotFound, "Provided Company does not exist");
            }

            Appointment? appointment;

            try
            {
                appointment = await _appointmentsService.GetValidatedAppointment(body, companyId, appointmentId);
            }
            catch (NotFoundException)
            {
                return NotFound("The service does not exist");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            try
            {
                await _appointmentsService.UpdateAsync(appointment);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to Update Appointment");
            }

            return Ok(appointment);
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("/cinematic/{companyId}/appointments/{appointmentId}")]
        public async Task<ActionResult> DeleteAppointment(Guid companyId, Guid appointmentId)
        {
            if (!await _context.CheckIfCompanyExists(companyId))
            {
                return StatusCode(StatusCodes.Status404NotFound, "Provided Company does not exist");
            }

            Appointment? appointment;

            try
            {
                appointment = await _appointmentsService.GetByPropertyAsync(x => x.Id == appointmentId);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            if (appointment == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }

            try
            {
                await _appointmentsService.DeleteAsync(appointment);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return StatusCode(StatusCodes.Status204NoContent);
        }
    }
}
