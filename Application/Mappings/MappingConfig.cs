using Application.Abstractions;                                              // твои DTO
using Application.Interactors.Loans.Commands.CreateLoan;
using Domain.Entities;
using Mapster;

namespace Application.Mappings
{
    public class MappingConfig : IRegister
    {
        public readonly ILoanNumberGenerator _generator;
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<CreateLoanCommand, Loan>()
                .Map(dest => dest.Id, src => Guid.NewGuid())
                .Map(dest => dest.Number, src => _generator.GenerateAsync())
                .Map(dest => dest.Amount, src => src.Amount)
                .Map(dest => dest.TermValue, src => src.TermValue)
                .Map(dest => dest.InterestValue, src => src.InterestValue)
                .Map(dest => dest.Status, _ => Status.Published)
                .Map(dest => dest.CreatedAt, _ => DateTimeOffset.UtcNow)
                .Map(dest => dest.ModifiedAt, _ => DateTimeOffset.UtcNow);
        }
    }
}