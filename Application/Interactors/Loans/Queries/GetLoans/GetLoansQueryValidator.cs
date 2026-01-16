using FluentValidation;
using Domain.Entities;

namespace Application.Interactors.Loans.Queries.GetLoans;

public sealed class GetLoansQueryValidator : AbstractValidator<GetLoansQuery>
{
    public GetLoansQueryValidator()
    {

        RuleFor(x => x.Status)
            .IsInEnum()
            .When(x => x.Status.HasValue)
            .WithMessage("Указан некорректный статус");

        RuleFor(x => x.MinAmount)
            .GreaterThanOrEqualTo(0)
            .When(x => x.MinAmount.HasValue)
            .WithMessage("Минимальная сумма не может быть отрицательной");

        RuleFor(x => x.MaxAmount)
            .GreaterThanOrEqualTo(0)
            .When(x => x.MaxAmount.HasValue)
            .WithMessage("Максимальная сумма не может быть отрицательной");

        RuleFor(x => x)
            .Must(x => !x.MinAmount.HasValue || !x.MaxAmount.HasValue || x.MinAmount <= x.MaxAmount)
            .When(x => x.MinAmount.HasValue && x.MaxAmount.HasValue)
            .WithMessage("Минимальная сумма не может быть больше максимальной");

        RuleFor(x => x.MinTerm)
            .GreaterThanOrEqualTo(0)
            .When(x => x.MinTerm.HasValue)
            .WithMessage("Минимальный срок не может быть отрицательным");

        RuleFor(x => x.MaxTerm)
            .GreaterThanOrEqualTo(0)
            .When(x => x.MaxTerm.HasValue)
            .WithMessage("Максимальный срок не может быть отрицательным");

        RuleFor(x => x)
            .Must(x => !x.MinTerm.HasValue || !x.MaxTerm.HasValue || x.MinTerm <= x.MaxTerm)
            .When(x => x.MinTerm.HasValue && x.MaxTerm.HasValue)
            .WithMessage("Минимальный срок не может быть больше максимального");
    }
}