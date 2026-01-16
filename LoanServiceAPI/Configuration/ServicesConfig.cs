using Application.Abstractions;
using Infrastructure.Services;

namespace LoanServiceAPI.Configuration
{
    public static class ServicesConfig
    {
        public static void AddServices(IServiceCollection services)
        {
            services.AddScoped<ILoanNumberGenerator, LoanNumberGenerator>();
        }
    }
}