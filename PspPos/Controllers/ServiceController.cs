using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.Json;
using Newtonsoft.Json;
using PspPos.Commons;
using PspPos.Data;
using PspPos.Models;
using PspPos.Models.DTO.Requests;
using PspPos.Repositories;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PspPos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private readonly IServiceRepository _serviceRepository;
        // private readonly IAppointmentRepository _appointmentRepository;
        private readonly ApplicationContext _context;

        public ServiceController(IServiceRepository serviceRepository/*, IAppointmentRepository appointmentRepository*/, ApplicationContext context)
        {
            _serviceRepository = serviceRepository;
            // _appointmentRepository = appointmentRepository;
            _context = context;
        }

        // GET: api/<ServiceController>
        [HttpGet("/cinematic/{companyId}/services")]
        public async Task<ActionResult<IEnumerable<Service>>> GetAllServices(Guid companyId)
        {
            IEnumerable<Service> services;

            if (await _context.CheckIfCompanyExists(companyId))
            {
                return StatusCode(StatusCodes.Status404NotFound, "Provided Company does not exist");
            }

            try
            {
                services = await _serviceRepository.GetAllByPropertyAsync(x => x.companyId == companyId);
            } 
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return Ok(services);

        }

        // POST api/<ServiceController>
        [HttpPost("/cinematic/{companyId}/services")]
        public async Task<ActionResult<Service>> CreateService(ServiceCreateRequest body, Guid companyId)
        {
            if (await _context.CheckIfCompanyExists(companyId))
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

           if (await _context.CheckIfCompanyExists(companyId))
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
            if (await _context.CheckIfCompanyExists(companyId))
            {
                return StatusCode(StatusCodes.Status404NotFound, "Provided Company does not exist");
            }

            var service = new Service { Id = serviceId, companyId = companyId, Description = body.Description, Price = body.Price, Name = body.Name, Tax = body.Price * TaxSystem.Tax };
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
            if (await _context.CheckIfCompanyExists(companyId))
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
            if (await _context.CheckIfCompanyExists(companyId))
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
    }
}
