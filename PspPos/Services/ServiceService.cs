using PspPos.Repositories;

namespace PspPos.Services
{
    public class ServiceService
    {
        private readonly IServiceRepository _repository;

        public ServiceService(IServiceRepository repository)
        {
            _repository = repository;
        }
    }
}
