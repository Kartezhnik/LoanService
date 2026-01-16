using Domain.Primitives;

namespace Domain.Entities
{
    public class Loan : Entity
    {
        public Status Status { get; set; }
        public string Number { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public int TermValue { get; set; }
        public decimal InterestValue { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset ModifiedAt { get; set; }

        public Loan(
            Guid id,
            Status status,
            string number,
            decimal amount,
            int termValue,
            decimal interestValue,
            DateTimeOffset createdAt,
            DateTimeOffset modifiedAt) : base(id) 
        {
            Id = id;
            Status = status;
            Number = number;
            Amount = amount;
            TermValue = termValue;
            InterestValue = interestValue;
            CreatedAt = createdAt;
            ModifiedAt = modifiedAt;
        }
    }

    public enum Status
    {
        Published,
        Unpublished
    }
}
