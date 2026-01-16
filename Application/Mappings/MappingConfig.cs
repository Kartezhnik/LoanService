using Application.DTOs;
using Application.Interactors.Loans.Commands.CreateLoan;
using Application.Interactors.Loans.Queries;
using Domain.Entities;
using Mapster;

namespace Application.Mappings
{
    public class MappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<CreateLoanCommand, Loan>()
                .Ignore(dest => dest.Id)
                .Ignore(dest => dest.Number)
                .Ignore(dest => dest.Status)
                .Ignore(dest => dest.CreatedAt)
                .Ignore(dest => dest.ModifiedAt)
                .Map(dest => dest.Amount, src => src.Amount)
                .Map(dest => dest.TermValue, src => src.TermValue)
                .Map(dest => dest.InterestValue, src => src.InterestValue);

            config.NewConfig<Loan, LoanResponse>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Number, src => src.Number)
                .Map(dest => dest.Status, src => src.Status)
                .Map(dest => dest.Amount, src => src.Amount)
                .Map(dest => dest.TermValue, src => src.TermValue)
                .Map(dest => dest.InterestValue, src => src.InterestValue)
                .Map(dest => dest.CreatedAt, src => src.CreatedAt)
                .Map(dest => dest.ModifiedAt, src => src.ModifiedAt);

            config.NewConfig<PagedResult<Loan>, PagedResult<LoanResponse>>()
                .Map(dest => dest.Items, src => src.Items.Adapt<IEnumerable<LoanResponse>>());
        }
    }
}