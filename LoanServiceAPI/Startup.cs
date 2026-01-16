using LoanServiceAPI.Configuration;
using LoanServiceAPI.Middleware;
using Mapster;
using MapsterMapper;
using Microsoft.OpenApi;

namespace LoanServiceAPI
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Loan Service API",
                    Version = "v1",
                    Description = "API для сервиса заявок на займ",
                });

                DbContextConfig.AddDbContext(services, _configuration);
                RepositoriesConfig.AddRepositories(services);
                ServicesConfig.AddServices(services);
                InteractorsConfig.AddInteractors(services);
                ValidatorsConfig.AddValidators(services);

                var config = TypeAdapterConfig.GlobalSettings;
                services.AddSingleton(config);
                services.AddScoped<IMapper, Mapper>();

            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Loan Service API V1");
                    c.RoutePrefix = "swagger"; 
                });
                app.ApplyMigrations();
            }

            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}