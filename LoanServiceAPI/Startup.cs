using LoanServiceAPI.Configuration;
using LoanServiceAPI.Middleware;
using Mapster;
using MapsterMapper;
using Microsoft.OpenApi;
using System.Text.Json;

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
            services.AddControllers()
        .AddJsonOptions(options =>
        {
            // 1. Делаем так, чтобы Enum возвращался текстом ("Published"), а не числом (1)
            options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
            // 2. Принудительно используем camelCase (amount вместо Amount)
            options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
            });
            services.AddEndpointsApiExplorer();

            DbContextConfig.AddDbContext(services, _configuration);
            RepositoriesConfig.AddRepositories(services);
            ServicesConfig.AddServices(services);
            InteractorsConfig.AddInteractors(services);
            ValidatorsConfig.AddValidators(services);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Loan Service API",
                    Version = "v1",
                    Description = "API для сервиса заявок на займ",
                });

            });

            var config = TypeAdapterConfig.GlobalSettings;
            services.AddSingleton(config);
            services.AddScoped<IMapper, Mapper>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.ApplyMigrations();

            //if (env.IsDevelopment())
            //{
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Loan Service API V1");
                    c.RoutePrefix = "swagger"; 
                });
            //}

            app.UseMiddleware<ExceptionHandlingMiddleware>();
            //app.UseHttpsRedirection();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}