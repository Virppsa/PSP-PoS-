using AutoMapper;
using PspPos.Infrastructure;
using PspPos.Models;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet("{companyId}")]
        public async Task<ActionResult<List<ItemViewModel>>> GetAllItems(Guid companyId)
        {
            var allCompanyItems = await _itemsService.GetAll(companyId);
            return Ok(_mapper.Map<List<ItemViewModel>>(allCompanyItems));
        }

        [HttpPost("{companyId}")]
        public async Task<ActionResult<ItemViewModel>> CreateItem(ItemPostModel item)
        {
            var createdItem = _mapper.Map<Item>(item);
            await _itemsService.Add(createdItem);

            return Ok(_mapper.Map<ItemViewModel>(createdItem));
        }

        [HttpGet("{companyId}/items/{itemId}")]
        public async Task<ActionResult<ItemViewModel>> GetItem(Guid companyId, Guid itemId)
        {
            var item = await _itemsService.Get(companyId, itemId);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<ItemViewModel>(item));
        }

        [HttpPut("{companyId}/items/{itemId}")]
        public async Task<ActionResult<Item>> UpdateCompany(Guid companyId, Guid itemId, ItemPostModel item)
        {
            var itemUpdateWith = _mapper.Map<Item>(item);
            itemUpdateWith.Id = itemId;
            itemUpdateWith.CompanyId = companyId;

            var updatedItem = await _itemsService.Update(itemUpdateWith);

            if (updatedItem == null)
            {
                return NotFound();
            }

            return Ok(updatedItem);
        }

        [HttpDelete("{companyId}/items/{itemId}")]
        public async Task<ActionResult> DeleteCompany(Guid companyId, Guid itemId)
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
    }
}