using PspPos.Models;

namespace PspPos.Infrastructure
{
    public interface IItemsService
    {
        public Task Add(Item item);
        public Task<Item?> Get(Guid companyId, Guid itemId);
        public Task<List<Item>> GetAll(Guid companyId);
        public Task<bool> Delete(Guid companyId, Guid itemId);
        public Task<Item?> Update(Item item);

        public Task AddDiscount(Guid companyId, Guid itemId, ServiceDiscount discount);

        public Task AddOption(ItemOption itemOption);
        public Task<ItemOption?> GetOption(Guid companyId, Guid itemOptionId);
        public Task<List<ItemOption>> GetAllOptions(Guid companyId);
        public Task<bool> DeleteOption(Guid companyId, Guid itemOptionId);
        public Task<ItemOption?> UpdateOption(ItemOption itemOption);

        public Task AddInventory(Inventory inventory);
        public Task<Inventory?> GetInventory(Guid companyId, Guid inventoryId);
        public Task<List<Inventory>> GetAllInventories(Guid companyId);
        public Task<bool> DeleteInventory(Guid companyId, Guid inventoryId);
        public Task<Inventory?> UpdateInventory(Inventory iteminventory);

    }
}
