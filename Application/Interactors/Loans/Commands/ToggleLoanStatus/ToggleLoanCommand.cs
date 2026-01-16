namespace Application.Interactors.Loans.Commands.ToggleLoanStatus
{
    public sealed record ToggleLoanCommand
    {
        public Guid Id;
        public ToggleLoanCommand(Guid id) => Id = id;
    }
}
