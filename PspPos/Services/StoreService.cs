using AutoMapper;
using PspPos.Data;
using PspPos.Models;
using PspPos.Infrastructure;
using System.ComponentModel.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace PspPos.Services
{
    public class StoreService : IStoreService
    {
        private readonly ApplicationContext _context;
        private readonly IMapper _mapper;

        public StoreService(ApplicationContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<Store>> GetStoresByCompanyID(Guid companyID)
        {
            return await _context.Stores
                                   .Where(u => u.CompanyId == companyID)
                                   .ToListAsync();
        }

        public async Task<Store> GetStoreByCompanyAndStoreID(Guid storeID, Guid companyID)
        {
            var store = await _context.Stores.FindAsync(storeID);

            if (!isStoreValid(store, companyID))
            {
                throw new KeyNotFoundException($"Store with ID {storeID} or CompanyID {companyID} not found.");
            }

            return store;
        }

        public async Task<Store> AddStore(StoreCreate? storeArgs)
        {
            if (storeArgs == null)
            {
                throw new ArgumentNullException(nameof(storeArgs));
            }

            var createStore = _mapper.Map<Store>(storeArgs);

            await _context.Stores.AddAsync(createStore);
            await _context.SaveChangesAsync();

            return createStore;
        }

        public async Task<Store> UpdateStore(Guid storeID, StoreUpdate? storeArgs)
        {
            var existingStore = await _context.Stores.FindAsync(storeID);
            if (existingStore == null)
            {
                throw new KeyNotFoundException($"Store with ID {storeID} not found.");
            }

            existingStore = _mapper.Map<Store>(existingStore);

            await _context.SaveChangesAsync();

            return existingStore;
        }

        public async Task DeleteStore(Guid storeID, Guid companyID) 
        {
            var existingStore = await _context.Stores.FindAsync(storeID);
            if (existingStore == null)
            {
                throw new KeyNotFoundException($"Store with ID {storeID} not found.");
            }

            if (existingStore.CompanyId == companyID)
            {
                throw new BadHttpRequestException("This store is not part of this company");
            }

            _context.Stores.Remove(existingStore);
            
            await _context.SaveChangesAsync();
        }


        private bool isStoreValid(Store? store, Guid companyID)
        {
            return store is not null && companyID == store.CompanyId;
        }
    }
}
