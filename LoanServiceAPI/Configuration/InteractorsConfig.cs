using Application.Interactors.Loans.Commands.CreateLoan;
using Application.Interactors.Loans.Commands.ToggleLoanStatus;
using Application.Interactors.Loans.Queries.GetLoans;

namespace LoanServiceAPI.Configuration
{
    public static class InteractorsConfig
    {
        public static void AddInteractors(IServiceCollection services)
        {
            services.AddScoped<CreateLoanCommandHandler>();
            services.AddScoped<ToggleLoanCommandHandler>();
            services.AddScoped<GetLoansQueryHandler>();
        }
    }
}