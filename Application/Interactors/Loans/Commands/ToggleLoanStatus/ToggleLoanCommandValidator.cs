using Application.Interactors.Loans.Commands.ToggleLoanStatus;
using FluentValidation;

namespace Application.Interactors.Loans.Commands.CreateLoan;

public sealed class ToggleLoanCommandValidator : AbstractValidator<ToggleLoanCommand>
{
    public ToggleLoanCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Должен быть указан Id");
    }
}