using Microsoft.AspNetCore.Mvc;
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
        private readonly IServiceRepository _serviceRepository;
        //private readonly IAppointmentRepository _appointmentsRepository;
        private readonly IAppointmentsService _appointmentsService;
        private readonly ApplicationContext _context;

        public ServiceController(IServiceRepository serviceRepository, ApplicationContext context, IAppointmentsService appointmentsService)
        {
            _serviceRepository = serviceRepository;
            _appointmentsService = appointmentsService;
            //_appointmentsService = _appointmentsService;
            _context = context;
        }

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
                services = await _serviceRepository.GetAllByPropertyAsync(x => x.companyId == companyId);
            } 
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return Ok(services);

        }

        [HttpPost("/cinematic/{companyId}/services")]
        public async Task<ActionResult<Service>> CreateService(ServiceCreateRequest body, Guid companyId)
        {
            if (!await _context.CheckIfCompanyExists(companyId))
            {
                return StatusCode(StatusCodes.Status404NotFound, "Provided Company does not exist");
            }

            var service = new Service { Id = Guid.NewGuid(), companyId = companyId, Description = body.Description, Price = body.Price, Name = body.Name };
            // calculate tax and add also
            await _serviceRepository.InsertAsync(service);

            return StatusCode(StatusCodes.Status200OK);

        }

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
               service = await _serviceRepository.GetByPropertyAsync(x => x.companyId == companyId && x.Id == serviceId);
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
                await _serviceRepository.UpdateAsync(service);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to Update Service");
            }

            return Ok(service);
        }


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
                service = await _serviceRepository.GetByPropertyAsync(x => x.Id == serviceId);
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
                await _serviceRepository.DeleteAsync(service);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return StatusCode(StatusCodes.Status204NoContent);
        }

        [HttpPut("/cinematic/{companyId}/services/{serviceId}/discount")]
        public async Task<ActionResult<ServiceDiscount>> AddDiscountToService(Guid companyId, Guid serviceId)
        {
            if (!await _context.CheckIfCompanyExists(companyId))
            {
                return StatusCode(StatusCodes.Status404NotFound, "Provided Company does not exist");
            }

            Service? service = await _serviceRepository.GetByPropertyAsync(x => x.Id == serviceId);
            if(service == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, "Provided Service does not exist");
            }

            // should be moved to service
            var discount = TaxSystem.CreateDefaultServiceDiscount(serviceId);
            var serviceWithDiscount = new Service { Id = serviceId, companyId = companyId, Description = service.Description, Price = service.Price, Name = service.Name, Tax = service.Tax, SerializedDiscount =  JsonConvert.SerializeObject(discount) };

            try
            {
               await _serviceRepository.UpdateAsync(service);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to Apply Discount to a Service");
            }

            return Ok(discount);
        }


        [HttpGet("/cinematic/{companyId}/appointments/")]
        public async Task<ActionResult<IEnumerable<GetAppointmentsRequest>>> GetAllAppointments([FromBody]GetAppointmentsRequest request, Guid companyId)
        {
            IEnumerable<Appointment> appointments;

            if (!await _context.CheckIfCompanyExists(companyId))
            {
                return StatusCode(StatusCodes.Status404NotFound, "Provided Company does not exist");
            }

            // validation for date being defined and parsable needed
            try
            {
                appointments = await _appointmentsService.GetAllRequestedAppointments(companyId, request.ServiceId, DateTime.Parse(request.LowerDateBoundary), DateTime.Parse(request.HigherDateBoundary));
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return Ok(appointments);
        }

        [HttpPost("/cinematic/{companyId}/appointments")]
        public async Task<ActionResult<Appointment>> CreateAppointment([FromBody] AppointmentCreateRequest body, Guid companyId)
        {
            if (!await _context.CheckIfCompanyExists(companyId))
            {
                return StatusCode(StatusCodes.Status404NotFound, "Provided Company does not exist");
            }

            // add validation for dates
            var appointment = new Appointment {
                Id = Guid.NewGuid(),
                CompanyId = companyId,
                StartDate = DateTime.Parse(body.StartDate),
                EndDate = DateTime.Parse(body.EndDate),
                WorkerId = body.WorkerId,
                StoreId = body.StoreId,
                OrderId = body.OrderId,
            };
           

            await _appointmentsService.InsertAsync(appointment);

            return StatusCode(StatusCodes.Status200OK);

        }

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

            if (appointment == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }

            return Ok(appointment);
        }

        [HttpPut("/cinematic/{companyId}/appointments/{appointmentId}")]
        public async Task<ActionResult<Appointment>> EditAppointment([FromBody] AppointmentCreateRequest body, Guid companyId, Guid appointmentId)
        {
            if (!await _context.CheckIfCompanyExists(companyId))
            {
                return StatusCode(StatusCodes.Status404NotFound, "Provided Company does not exist");
            }

            // validation for dates
            var appointment = new Appointment
            {
                Id = appointmentId,
                CompanyId = companyId,
                StartDate = DateTime.Parse(body.StartDate),
                EndDate = DateTime.Parse(body.EndDate),
                WorkerId = body.WorkerId,
                StoreId = body.StoreId,
                OrderId = body.OrderId,
            };
           
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
