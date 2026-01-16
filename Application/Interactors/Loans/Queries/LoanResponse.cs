using Domain.Entities;

namespace Application.Interactors.Loans.Queries
{
    public sealed record LoanResponse
    {
        public Guid Id { get; set; }
        public Status Status { get; set; }
        public string Number { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public int TermValue { get; set; }
        public decimal InterestValue { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset ModifiedAt { get; set; }

        public LoanResponse(
            Guid id,
            Status status,
            string numder,
            decimal amount,
            int termValue,
            decimal interestValue,
            DateTimeOffset createdAt,
            DateTimeOffset modifiedAt)
        {
            Id = id;
            Status = status;
            Number = numder;
            Amount = amount;
            TermValue = termValue;
            InterestValue = interestValue;
            CreatedAt = createdAt;
            ModifiedAt = modifiedAt;
        }
    }
}
