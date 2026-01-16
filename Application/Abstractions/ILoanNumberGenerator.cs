namespace Application.Abstractions
{
    public interface ILoanNumberGenerator
    {
        public Task<string> GenerateAsync();
    }
}
