using AutoMapper;
using PspPos.Data;
using PspPos.Infrastructure;
using PspPos.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace PspPos.Services
{
    public class SampleService : ISampleService
    {
        private readonly ApplicationContext _context;
        private readonly IMapper _mapper;

        public SampleService(ApplicationContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task Add(Sample sample)
        {
            await _context.Samples.AddAsync(sample);
        }

        public async Task<Sample> Get(int id)
        {
            var sample = await _context.Samples.FindAsync(id);
            if (sample == null)
            {
                throw new Exception("Sample not found");
            }
            return sample;
        }

        public async Task<List<Sample>> GetAll()
        {
            return await _context.Samples.ToListAsync();
        }

        public async Task<Sample> Delete(int id)
        {
            var sample = await _context.Samples.FindAsync(id);
            if (sample == null)
            {
                throw new Exception("Sample not found");
            }
            _context.Samples.Remove(sample);

            await _context.SaveChangesAsync();
            return sample;
        }

        public async Task SaveAll()
        {
            await _context.SaveChangesAsync();
        }

    }
}
