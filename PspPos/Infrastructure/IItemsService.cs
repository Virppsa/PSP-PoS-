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

    }
}
