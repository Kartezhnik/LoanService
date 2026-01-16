using Domain.Entities;

namespace Application.Interactors.Loans.Commands.CreateLoan
{
    public sealed record CreateLoanCommand
    {
        public decimal Amount { get; init; }
        public int TermValue { get; init; }
        public decimal InterestValue { get; init; }

        public CreateLoanCommand(
            decimal amount,
            int termValue,
            decimal interestValue)
        {
            Amount = amount;
            TermValue = termValue;
            InterestValue = interestValue;
        }
    }
}
