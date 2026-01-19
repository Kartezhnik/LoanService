using Domain.Entities;

namespace Application.Interactors.Loans.Commands.CreateLoan
{
    public sealed record CreateLoanCommand
    {
        public string Number { get; init; }
        public decimal Amount { get; init; }
        public int TermValue { get; init; }
        public decimal InterestValue { get; init; }

        public CreateLoanCommand(
            string number,
            decimal amount,
            int termValue,
            decimal interestValue)
        {
            Number = number;
            Amount = amount;
            TermValue = termValue;
            InterestValue = interestValue;
        }
    }
}
