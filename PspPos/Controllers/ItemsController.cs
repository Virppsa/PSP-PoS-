using AutoMapper;
using PspPos.Infrastructure;
using PspPos.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PspPos.Commons;

namespace PspPos.Controllers
{
    [ApiController]
    [Route("cinematic/")]
    public class ItemsController : ControllerBase
    {

        private readonly IItemsService _itemsService;
        private readonly IMapper _mapper;

        public ItemsController(IItemsService itemsService, IMapper mapper)
        {
            _itemsService = itemsService;
            _mapper = mapper;
        }

        // Items CRUD -------------------------------------------------------------------------

        [HttpGet("{companyId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<ItemViewModel>>> GetAllItems(Guid companyId)
        {
            try
            {

                var allCompanyItems = await _itemsService.GetAll(companyId);
                return Ok(_mapper.Map<List<ItemViewModel>>(allCompanyItems));

            } 
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("{companyId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ItemViewModel>> CreateItem(Guid companyId, ItemPostModel item)
        {
            try 
            { 

                var createdItem = _mapper.Map<Item>(item);
                createdItem.CompanyId = companyId;
                await _itemsService.Add(createdItem);

                return Ok(_mapper.Map<ItemViewModel>(createdItem));

            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{companyId}/items/{itemId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ItemViewModel>> GetItem(Guid companyId, Guid itemId)
        {
            try 
            { 

                var item = await _itemsService.Get(companyId, itemId);
                if (item == null)
                {
                    return NotFound();
                }
                return Ok(_mapper.Map<ItemViewModel>(item));

            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut("{companyId}/items/{itemId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ItemViewModel>> UpdateItem(Guid companyId, Guid itemId, ItemPostModel item)
        {
            try { 

                var itemUpdateWith = _mapper.Map<Item>(item);
                itemUpdateWith.Id = itemId;
                itemUpdateWith.CompanyId = companyId;

                var updatedItem = await _itemsService.Update(itemUpdateWith);

                if (updatedItem == null)
                {
                    return NotFound();
                }

                return Ok(_mapper.Map<ItemViewModel>(updatedItem));

            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{companyId}/items/{itemId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteCompany(Guid companyId, Guid itemId)
        {
            try 
            { 

                bool deleted = await _itemsService.Delete(companyId, itemId);
                if (deleted == false)
                {
                    return NotFound();
                }
                else { 
                    return NoContent();
                }

            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // Items add discount -------------------------------------------------------------------------

        [HttpPost("{companyId}/items/{itemId}/discounts")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ServiceDiscount>> CreateDiscount(Guid companyId, Guid itemId)
        {
            try
            {
                var discount = new ServiceDiscount();

                await _itemsService.AddDiscount(companyId, itemId, discount);

                return Ok(discount);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // Item option CRUD ---------------------------------------------------------------------------

    }
}