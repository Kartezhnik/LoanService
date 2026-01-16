using FluentValidation;

namespace Application.Interactors.Loans.Commands.CreateLoan;

public sealed class CreateLoanCommandValidator : AbstractValidator<CreateLoanCommand>
{
    public CreateLoanCommandValidator()
    {
        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .WithMessage("Сумма должна быть больше 0");

        RuleFor(x => x.TermValue)
            .GreaterThan(0)
            .WithMessage("Срок займа должен быть больше 0");

        RuleFor(x => x.InterestValue)
            .GreaterThan(0)
            .WithMessage("Процентная ставка должна быть больше 0");
    }
}