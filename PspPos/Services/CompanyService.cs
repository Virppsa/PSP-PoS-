using AutoMapper;
using PspPos.Data;
using PspPos.Infrastructure;
using PspPos.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.Design;
using System.Collections.Concurrent;

namespace PspPos.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ApplicationContext _context;
        private readonly IMapper _mapper;

        public CompanyService(ApplicationContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task Add(Company company)
        {
            await _context.Companies.AddAsync(company);
            (await _context.GetAvailableCompanies()).Add(company.Id);
            await _context.SaveChangesAsync();
        }

        public async Task<Company?> Get(int id)
        {
            return await _context.Companies.FindAsync(id);
        }

        public async Task<List<Company>> GetAll()
        {
            return await _context.Companies.ToListAsync();
        }

        public async Task<bool> Delete(int id)
        {
            var company = await _context.Companies.FindAsync(id);
            if (company == null)
            {
                return false; // Not found
            }
            _context.Companies.Remove(company);

            (await _context.GetAvailableCompanies()).Remove(id);

            await _context.SaveChangesAsync();
            return true; // Successful
        }

        public async Task<Company?> Update(Company company)
        {
            var companyToUpdate = await Get(company.Id);
            if (companyToUpdate == null)
            {
                return null;
            }

            companyToUpdate.Name = company.Name;
            companyToUpdate.Email = company.Email;
            await _context.SaveChangesAsync();
            return companyToUpdate;
        }
    }
}
