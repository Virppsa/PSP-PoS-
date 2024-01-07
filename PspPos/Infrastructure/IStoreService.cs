using PspPos.Models;

namespace PspPos.Infrastructure
{
    public interface IStoreService
    {
        public Task<List<Store>> GetStoresByCompanyID(Guid companyID);
        public Task<Store> GetStoreByCompanyAndStoreID(Guid storeID, Guid companyID);
        public Task<Store> AddStore(StoreCreate? storeArgs);
        public Task<Store> UpdateStore(Guid storeID, StoreUpdate? storeArgs);
        public Task DeleteStore(Guid storeID, Guid companyID);
    }
}
