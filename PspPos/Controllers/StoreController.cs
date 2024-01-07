using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PspPos.Infrastructure;
using PspPos.Models;
using PspPos.Models.DTO.Requests;
using PspPos.Services;
using System.ComponentModel.DataAnnotations;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace PspPos.Controllers
{
    [ApiController]
    [Route("cinematic/")]
    public class StoreController : ControllerBase
    {

        private readonly IStoreService _storeService;
        private readonly IMapper _mapper;

        public StoreController(IStoreService storeService, IMapper mapper)
        {
            _storeService = storeService;
            _mapper = mapper;
        }

        [HttpGet("{companyID}/stores")]
        public async Task<ActionResult<List<Store>>> GetCompanyStores([Required] Guid companyID)
        {
            try
            {
                return Ok(await _storeService.GetStoresByCompanyID(companyID));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                   "Error retrieving data from the database" + ex);
            }
        }

        

        [HttpGet("{companyID}/stores/{storeID}")]
        public async Task<ActionResult<User>> GetCompanyStoreByCompanyAndStoreID([Required] Guid companyID, [Required] Guid storeID)
        {
            try
            {
                return Ok(await _storeService.GetStoreByCompanyAndStoreID(storeID, companyID));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                  "Error retrieving data from the database" + e);
            }
        }

        [HttpPost("{companyID}/stores")]
        public async Task<ActionResult<Store>> CreateStore([Required] Guid companyID, [Required] StoreCreate storeArgs)
        {
            try
            {
                return Ok(await _storeService.AddStore(storeArgs));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                  "Error creating store" + e);
            }
        }

        [HttpPut("{companyID}/stores/{storeID}")]
        public async Task<ActionResult<Store>> UpdateStore([Required] Guid companyID, [Required] Guid storeID, [Required] StoreUpdate storeArgs)
        {
            try
            {
                return Ok(await _storeService.UpdateStore(storeID, storeArgs));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                  "Error updating store" + e);
            }
        }

        [HttpDelete("{companyID}/stores/{storeID}")]
        public async Task<ActionResult> DeleteStore([Required] Guid companyID, [Required] Guid storeID)
        {
            try
            {
                await _storeService.DeleteStore(storeID, companyID);

                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                  "Error deleting store" + e);
            }
        }
    }
}
