using Domain.Abstractions;
using Domain.Entities;

namespace Application.Interactors.Loans.Commands.ToggleLoanStatus
{
    public class ToggleLoanCommandHandler
    {
        private readonly IRepository<Loan> _repository;
        
        public ToggleLoanCommandHandler(IRepository<Loan> repository)
        {
            _repository = repository;
        }

        public async Task<Guid> Handle(ToggleLoanCommand command, CancellationToken cancellationToken)
        {
            IReadOnlyList<Loan> loans = await _repository.GetByFilterAsync(loan => loan.Id == command.Id, cancellationToken);

            Loan loan = loans.First();

            loan.Status = loan.Status == Status.Published ? Status.Unpublished : Status.Published;
            loan.ModifiedAt = DateTimeOffset.UtcNow;
            await _repository.Update(loan);

            return loan.Id;
        }
    }
}
