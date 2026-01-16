using Domain.Entities;

namespace Application.Interactors.Loans.Commands.CreateLoan
{
    public sealed record CreateLoanCommand
    {
        public decimal Amount { get; set; }
        public int TermValue { get; set; }
        public decimal InterestValue { get; set; }

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
