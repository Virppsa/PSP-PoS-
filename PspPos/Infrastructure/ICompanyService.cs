using PspPos.Models;

namespace PspPos.Infrastructure
{
    public interface ICompanyService
    {
        public Task Add(Company company);
        public Task<Company?> Get(int id);
        public Task<List<Company>> GetAll();
        public Task<bool> Delete(int id);
        public Task<Company?> Update(Company company);

    }
}
