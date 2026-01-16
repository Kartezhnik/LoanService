using Application.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class LoanNumberGenerator : ILoanNumberGenerator
    {
        private readonly ApplicationDbContext _context;

        public LoanNumberGenerator(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<string> GenerateAsync()
        {
            int year = DateTimeOffset.UtcNow.Year;   

            int countThisYear = await _context.Loans
                .Where(l => l.CreatedAt.Year == year)
                .CountAsync();

            int nextNumber = countThisYear + 1;

            return $"{year}-{nextNumber:000000}";
        }
    }
}
