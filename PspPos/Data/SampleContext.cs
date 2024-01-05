using PspPos.Models;
using Microsoft.EntityFrameworkCore;

namespace PspPos.Data;

public class SampleContext : DbContext
{
    public SampleContext(DbContextOptions<SampleContext> options) : base(options)
    { }

    public DbSet<Sample> Samples => Set<Sample>();

    public async Task<Sample[]> GetAllSamplesAsync()
    {
        return await Samples.ToArrayAsync();
    }

    public async Task<Sample> GetSampleAsync(int id)
    {
        var sample = await Samples.FindAsync(id);
        if (sample is null)
            throw new Exception($"Could not find sample with id {id}");

        return sample;
    }

    public async Task AddSampleAsync(Sample sample)
    {
        await Samples.AddAsync(sample);
        await this.SaveChangesAsync();
    }

    public async Task<Sample> UpdateSampleAsync(Sample updatedSample)
    {
        var sample = await GetSampleAsync(updatedSample.Id);
        sample.Location = updatedSample.Location;
        await SaveChangesAsync();
        return sample;
    }

    public async Task DeleteSampleAsync(int id)
    {
        var sample = await GetSampleAsync(id);
        Samples.Remove(sample);
        await SaveChangesAsync();
    }
}
