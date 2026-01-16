namespace Application.Interactors.Loans.Commands.ToggleLoanStatus
{
    public sealed record ToggleLoanCommand
    {
        public Guid Id { get; set; }
        public ToggleLoanCommand(Guid id) => Id = id;
        public ToggleLoanCommand() { }
    }
}
