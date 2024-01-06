using AutoMapper;
using PspPos.Data;
using PspPos.Infrastructure;
using PspPos.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.Design;

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
            await _context.SaveChangesAsync();
        }

        public async Task<Company> Get(int id)
        {
            var company = await _context.Companies.FindAsync(id);
            if (company == null)
            {
                throw new Exception("Company not found");
            }
            return company;
        }

        public async Task<List<Company>> GetAll()
        {
            return await _context.Companies.ToListAsync();
        }

        public async Task<Company> Delete(int id)
        {
            var company = await _context.Companies.FindAsync(id);
            if (company == null)
            {
                throw new Exception("Company not found");
            }
            _context.Companies.Remove(company);

            await _context.SaveChangesAsync();
            return company;
        }

        public async Task<Company> Update(Company company)
        {
            var companyToUpdate = await Get(company.Id);
            if (companyToUpdate == null)
            {
                throw new Exception("Company not found");
            }

            companyToUpdate.Name = company.Name;
            companyToUpdate.Email = company.Email;
            await _context.SaveChangesAsync();
            return companyToUpdate;
        }
    }
}
