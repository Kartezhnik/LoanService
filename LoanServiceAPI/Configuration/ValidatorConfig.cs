using Application.Interactors.Loans.Commands.CreateLoan;
using Application.Interactors.Loans.Commands.ToggleLoanStatus;
using Application.Interactors.Loans.Queries.GetLoans;
using FluentValidation;

namespace LoanServiceAPI.Configuration
{
    public static class ValidatorsConfig
    {
        public static void AddValidators(IServiceCollection services)
        {
            services.AddScoped<IValidator<CreateLoanCommand>, CreateLoanCommandValidator>();
            services.AddScoped<IValidator<ToggleLoanCommand>, ToggleLoanCommandValidator>();
            services.AddScoped<IValidator<GetLoansQuery>, GetLoansQueryValidator>();
        }
    }
}