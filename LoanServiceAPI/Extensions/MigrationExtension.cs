using Microsoft.EntityFrameworkCore;

namespace LoanServiceAPI.Configuration
{
    public static class MigrationExtension
    {
        public static void ApplyMigrations(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<Infrastructure.ApplicationDbContext>();
                context?.Database.Migrate();
            }
        }
    }
}