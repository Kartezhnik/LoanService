using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace LoanServiceAPI.Configuration
{
    public static class MigrationExtensions
    {
        public static void ApplyMigrations(this IApplicationBuilder app)
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            try
            {
                dbContext.Database.Migrate();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка миграции: {ex.Message}");
            }
        }
    }
}