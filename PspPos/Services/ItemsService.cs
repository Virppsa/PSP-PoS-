using AutoMapper;
using PspPos.Data;
using PspPos.Infrastructure;
using PspPos.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.Design;
using PspPos.Commons;

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
    }
}
