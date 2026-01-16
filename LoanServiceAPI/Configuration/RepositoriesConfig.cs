using Domain.Abstractions;
using Infrastructure;

namespace LoanServiceAPI.Configuration
{
    public static class RepositoriesConfig
    {
        public static void AddRepositories(IServiceCollection service)
        {
            service.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        }
    }
}