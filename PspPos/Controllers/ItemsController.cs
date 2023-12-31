using AutoMapper;
using PspPos.Infrastructure;
using PspPos.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PspPos.Commons;
using System.Reflection.Metadata.Ecma335;

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
        public async Task<ActionResult> DeleteItem(Guid companyId, Guid itemId)
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

        [HttpGet("{companyId}/itemOptions")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<ItemOptionViewModel>>> GetAllItemOptions(Guid companyId)
        {
            try
            {
                var allCompanyItemOptions = await _itemsService.GetAllOptions(companyId);
                return Ok(_mapper.Map<List<ItemOptionViewModel>>(allCompanyItemOptions));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("{companyId}/itemOptions")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ItemOptionViewModel>> CreateItemOption(Guid companyId, ItemOptionPostModel itemOption)
        {
            try
            {

                var createdItemOption = _mapper.Map<ItemOption>(itemOption);
                createdItemOption.CompanyId = companyId;
                await _itemsService.AddOption(createdItemOption);

                return Ok(_mapper.Map<ItemOptionViewModel>(createdItemOption));

            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{companyId}/itemOptions/{itemOptionId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ItemOptionViewModel>> GetItemOption(Guid companyId, Guid itemOptionId)
        {
            try
            {
                var itemOption = await _itemsService.GetOption(companyId, itemOptionId);
                if (itemOption == null)
                {
                    return NotFound();
                }
                return Ok(_mapper.Map<ItemOptionViewModel>(itemOption));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut("{companyId}/itemOptions/{itemOptionId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ItemOptionViewModel>> UpdateItemOption(Guid companyId, Guid itemOptionId, ItemOptionPostModel itemOption)
        {
            try
            {

                var itemOptionUpdateWith = _mapper.Map<ItemOption>(itemOption);
                itemOptionUpdateWith.Id = itemOptionId;
                itemOptionUpdateWith.CompanyId = companyId;

                var updatedItemOption = await _itemsService.UpdateOption(itemOptionUpdateWith);

                if (updatedItemOption == null)
                {
                    return NotFound();
                }

                return Ok(_mapper.Map<ItemOptionViewModel>(updatedItemOption));

            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{companyId}/itemOptions/{itemOptionId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteItemOption(Guid companyId, Guid itemOptionId)
        {
            try
            {
                bool deleted = await _itemsService.DeleteOption(companyId, itemOptionId);
                if (deleted == false)
                {
                    return NotFound();
                }
                else
                {
                    return NoContent();
                }
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // Inventory CRUD ---------------------------------------------------------------------------

        [HttpGet("{companyId}/inventory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<InventoryViewModel>>> GetAllInventories(Guid companyId)
        {
            //LATER should accept query params to filter based on storeId and itemId
            try
            {
                var allInventories = await _itemsService.GetAllInventories(companyId);
                return Ok(_mapper.Map<List<InventoryViewModel>>(allInventories));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("{companyId}/inventory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<InventoryViewModel>> CreateInventory(Guid companyId, InventoryPostModel inventory)
        {
            //this is important
            try
            {
                var createdInventory = _mapper.Map<Inventory>(inventory);
                createdInventory.CompanyId = companyId;
                await _itemsService.AddInventory(createdInventory);

                return Ok(_mapper.Map<InventoryViewModel>(createdInventory));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{companyId}/inventory/{inventoryId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<InventoryViewModel>> GetInventory(Guid companyId, Guid inventoryId)
        {
            try
            {
                var inventory = await _itemsService.GetInventory(companyId, inventoryId);
                if (inventory == null)
                {
                    return NotFound();
                }
                return Ok(_mapper.Map<InventoryViewModel>(inventory));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut("{companyId}/inventory/{inventoryId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<InventoryViewModel>> UpdateInventory(Guid companyId, Guid inventoryId, InventoryPostModel inventory)
        {
            try
            {

                var inventoryUpdateWith = _mapper.Map<Inventory>(inventory);
                inventoryUpdateWith.Id = inventoryId;
                inventoryUpdateWith.CompanyId = companyId;

                var updatedInventory = await _itemsService.UpdateInventory(inventoryUpdateWith);

                if (updatedInventory == null)
                {
                    return NotFound();
                }

                return Ok(_mapper.Map<InventoryViewModel>(updatedInventory));

            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{companyId}/inventory/{inventoryId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteInventory(Guid companyId, Guid inventoryId)
        {
            try
            {
                bool deleted = await _itemsService.DeleteInventory(companyId, inventoryId);
                if (deleted == false)
                {
                    return NotFound();
                }
                else
                {
                    return NoContent();
                }
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}