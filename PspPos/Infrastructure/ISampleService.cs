using PspPos.Models;

namespace PspPos.Infrastructure
{
    public interface ISampleService
    {
        public Task Add(Sample sample);
        public Task<Sample> Get(int id);
        public Task<List<Sample>> GetAll();
        public Task<Sample> Delete(int id);
        public Task SaveAll();

    }
}
