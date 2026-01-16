using Application.DTOs;
using Domain.Abstractions;
using Domain.Entities;
using FluentValidation;
using Mapster;
using System.Linq.Expressions;

namespace Application.Interactors.Loans.Queries.GetLoans
{
    public class GetLoansQueryHandler
    {
        private readonly IRepository<Loan> _repository;
        private readonly IValidator<GetLoansQuery> _validator;  

        public GetLoansQueryHandler(
            IRepository<Loan> repository,
            IValidator<GetLoansQuery> validator)
        {
            _repository = repository;
            _validator = validator;
        }

        public async Task<PagedResult<LoanResponse>> Handle(GetLoansQuery query, CancellationToken cancellationToken)
        {
            await _validator.ValidateAndThrowAsync(query, cancellationToken);

            Expression<Func<Loan, bool>> filter = l => true;

            if (query.Status.HasValue)
                filter = CombineFilters(filter, l => l.Status == query.Status.Value);

            if (query.MinAmount.HasValue)
                filter = CombineFilters(filter, l => l.Amount >= query.MinAmount.Value);

            if (query.MaxAmount.HasValue)
                filter = CombineFilters(filter, l => l.Amount <= query.MaxAmount.Value);

            if (query.MinTerm.HasValue)
                filter = CombineFilters(filter, l => l.TermValue >= query.MinTerm.Value);

            if (query.MaxTerm.HasValue)
                filter = CombineFilters(filter, l => l.TermValue <= query.MaxTerm.Value);

            var totalCount = await _repository.CountAsync(filter, cancellationToken);

            var loans = await _repository.GetByFilterAsync(
                filter,
                query.PageNumber,
                query.PageSize,
                cancellationToken);

            foreach (var loan in loans)
            {
                Console.WriteLine($"DEBUG: LoanId: {loan.Id}, Amount: {loan.Amount}, Status: {loan.Status}");
            }

            var loanResponses = loans.Adapt<IReadOnlyList<LoanResponse>>();

            return new PagedResult<LoanResponse>(
                items: loanResponses,
                totalCount: totalCount,
                pageNumber: query.PageNumber,
                pageSize: query.PageSize);

        }

        private static Expression<Func<T, bool>> CombineFilters<T>(
        Expression<Func<T, bool>> first,
        Expression<Func<T, bool>> second)
        {
            var parameter = first.Parameters[0];
            var body = Expression.AndAlso(first.Body, Expression.Invoke(second, parameter));
            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }
    }
}
