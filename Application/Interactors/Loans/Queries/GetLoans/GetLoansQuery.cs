using Domain.Entities;

namespace Application.Interactors.Loans.Queries.GetLoans
{
    public sealed record GetLoansQuery
    {
        public Status? Status { get; init; }
        public decimal? MinAmount { get; init; }
        public decimal? MaxAmount { get; init; }
        public int? MinTerm { get; init; }
        public int? MaxTerm { get; init; }

        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = 20;

        public GetLoansQuery(
            Status? status,
            decimal? minAmount,
            decimal? maxAmount,
            int? minTerm,
            int? maxTerm,
            int pageNumber = 1,
            int pageSize = 20) 
        {
            Status = status;
            MinAmount = minAmount;
            MaxAmount = maxAmount;
            MinTerm = minTerm;
            MaxTerm = maxTerm;
            PageNumber = Math.Max(1, pageNumber);    
            PageSize = Math.Clamp(pageSize, 5, 100);
        }

        public GetLoansQuery() { }
    }
}
