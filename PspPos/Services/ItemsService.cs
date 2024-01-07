using AutoMapper;
using PspPos.Data;
using PspPos.Infrastructure;
using PspPos.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.Design;
using PspPos.Commons;
using Newtonsoft.Json;

namespace PspPos.Services
{
    public class ItemsService : IItemsService
    {
        private readonly ApplicationContext _context;
        private readonly IMapper _mapper;

        public ItemsService(ApplicationContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // Item CRUD --------------------------------------------------------------------------
        public async Task Add(Item item)
        {
            if (await _context.CheckIfCompanyExists(item.CompanyId) == false)
            {
                throw new NotFoundException($"Company with id={item.CompanyId} doesn't exist");
            } 
            else
            {
                item.Tax = item.Price * TaxSystem.TaxMultiplier;
                await _context.Items.AddAsync(item);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Item?> Get(Guid companyId, Guid itemId)
        {
            if (await _context.CheckIfCompanyExists(companyId) == false)
            {
                throw new NotFoundException($"Company with id={companyId} doesn't exist");
            }
            else
            {
                return await _context.Items.FindAsync(itemId);
            }
        }

        public async Task<List<Item>> GetAll(Guid companyId)
        {
            if (await _context.CheckIfCompanyExists(companyId) == false)
            {
                throw new NotFoundException($"Company with id={companyId} doesn't exist");
            }
            else
            {
                var companyItems = from item in await _context.Items.ToListAsync()
                                   where item.CompanyId == companyId
                                   select item;
                return companyItems.ToList();
            }
        }

        public async Task<bool> Delete(Guid companyId, Guid id)
        {
            if (await _context.CheckIfCompanyExists(companyId) == false)
            {
                throw new NotFoundException($"Company with id={companyId} doesn't exist");
            }
            else
            {
                var item = await _context.Items.FindAsync(id);
                if (item == null)
                {
                    return false; // Not found
                }

                _context.Items.Remove(item);

                await _context.SaveChangesAsync();
                return true; // Successful
            }
        }

        public async Task<Item?> Update(Item item)
        {
            if (await _context.CheckIfCompanyExists(item.CompanyId) == false)
            {
                throw new NotFoundException($"Company with id={item.CompanyId} doesn't exist");
            }
            else
            {
                var itemToUpdate = await Get(item.CompanyId, item.Id);
                if (itemToUpdate == null)
                {
                    return null;
                }

                itemToUpdate.Name = item.Name;
                itemToUpdate.Description = item.Description;
                itemToUpdate.Price = item.Price;
                itemToUpdate.Tax = item.Price * TaxSystem.TaxMultiplier;

                await _context.SaveChangesAsync();
                return itemToUpdate;
            }
        }

        // Add item discount --------------------------------------------------------------------------

        public async Task AddDiscount(Guid companyId, Guid itemId, ServiceDiscount discount)
        {
            if (await _context.CheckIfCompanyExists(companyId) == false)
            {
                throw new NotFoundException($"Company with id={companyId} doesn't exist");
            }
            else
            {
                var itemToUpdate = await Get(companyId, itemId) ?? throw new NotFoundException($"Item with id={itemId} not found");
                itemToUpdate.SerializedDiscount = JsonConvert.SerializeObject(discount);

                await _context.SaveChangesAsync();
            }
        }


        // ItemOption CRUD --------------------------------------------------------------------------
        public async Task AddOption(ItemOption itemOption)
        {
            throw new NotImplementedException();
        }

        public async Task<ItemOption?> GetOption(Guid companyId, Guid itemOptionId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ItemOption>> GetAllOptions(Guid companyId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteOption(Guid companyId, Guid itemOptionId)
        {
            throw new NotImplementedException();
        }

        public async Task<ItemOption?> UpdateOption(ItemOption itemOption)
        {
            throw new NotImplementedException();
        }


        // Inventory CRUD --------------------------------------------------------------------------

        public async Task AddInventory(Inventory inventory)
        {
            if (await _context.CheckIfCompanyExists(inventory.CompanyId) == false)
            {
                throw new NotFoundException($"Company with id={inventory.CompanyId} doesn't exist");
            }
            else
            {
                // TODO: when stores service merged, implement this
                if (false)
                {
                    throw new NotFoundException($"Store with id={inventory.StoreId} doesn't exist");
                }

                var item = Get(inventory.CompanyId, inventory.ItemId);
                if (item is null)
                {
                    throw new NotFoundException($"Item with id={inventory.ItemId} doesn't exist");
                }
                
                await _context.Inventories.AddAsync(inventory);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Inventory?> GetInventory(Guid companyId, Guid inventoryId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Inventory>> GetAllInventories(Guid companyId)
        {
            if (await _context.CheckIfCompanyExists(companyId) == false)
            {
                throw new NotFoundException($"Company with id={companyId} doesn't exist");
            } 
            else
            {
                var companyInventories = from inventory in await _context.Inventories.ToListAsync()
                                   where inventory.CompanyId == companyId
                                   select inventory;
                return companyInventories.ToList();
            }
        }

        public async Task<bool> DeleteInventory(Guid companyId, Guid inventoryId)
        {
            throw new NotImplementedException();
        }

        public async Task<Inventory?> UpdateInventory(Inventory iteminventory)
        {
            throw new NotImplementedException();
        }
    }
}
