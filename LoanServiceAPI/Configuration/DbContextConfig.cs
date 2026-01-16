using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace LoanServiceAPI.Configuration
{
    public static class DbContextConfig
    {
        public static void AddDbContext(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
        }
    }
}