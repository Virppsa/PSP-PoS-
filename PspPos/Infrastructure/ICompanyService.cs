using PspPos.Models;

namespace PspPos.Infrastructure
{
    public interface ICompanyService
    {
        public Task Add(Company company);
        public Task<Company?> Get(Guid id);
        public Task<List<Company>> GetAll();
        public Task<bool> Delete(Guid id);
        public Task<Company?> Update(Company company);

    }
}
