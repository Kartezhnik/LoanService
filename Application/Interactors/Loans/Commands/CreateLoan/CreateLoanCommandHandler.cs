using Domain.Abstractions;
using Domain.Entities;
using FluentValidation;
using MapsterMapper;

namespace Application.Interactors.Loans.Commands.CreateLoan
{
    public class CreateLoanCommandHandler
    {
        private readonly IRepository<Loan> _repository;
        private readonly IValidator<CreateLoanCommand> _validator;
        private readonly IMapper _mapper;

        public CreateLoanCommandHandler(
            IRepository<Loan> repository,
            IValidator<CreateLoanCommand> validator,
            IMapper mapper)
        {
            _repository = repository;
            _validator = validator;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(CreateLoanCommand command, CancellationToken cancellationToken)
        {
            await _validator.ValidateAndThrowAsync(command, cancellationToken);

            Loan loan = _mapper.Map<Loan>(command);

            await _repository.CreateAsync(loan, cancellationToken);

            return loan.Id;
        }
    }
}
