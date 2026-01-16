using Domain.Abstractions;
using Domain.Entities;
using FluentValidation;

namespace Application.Interactors.Loans.Commands.ToggleLoanStatus
{
    public class ToggleLoanCommandHandler
    {
        private readonly IRepository<Loan> _repository;
        private readonly IValidator<ToggleLoanCommand> _validator;
        
        public ToggleLoanCommandHandler(
            IRepository<Loan> repository,
            IValidator<ToggleLoanCommand> validator)
        {
            _repository = repository;
            _validator = validator;
        }

        public async Task<Guid> Handle(ToggleLoanCommand command, CancellationToken cancellationToken)
        {
            await _validator.ValidateAndThrowAsync(command, cancellationToken);

            IReadOnlyList<Loan> loans = await _repository.GetByFilterAsync(loan => loan.Id == command.Id, cancellationToken);

            Loan loan = loans.First();

            loan.Status = loan.Status == Status.Published ? Status.Unpublished : Status.Published;
            loan.ModifiedAt = DateTimeOffset.UtcNow;
            await _repository.Update(loan);

            return loan.Id;
        }
    }
}
