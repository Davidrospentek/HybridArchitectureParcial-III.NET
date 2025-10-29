using Application.Repositories;
using Infrastructure.Persistence;
using Infrastructure.Repositories.Sql;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Registrations
{
    /// <summary>
    /// Registro de servicios de infraestructura para el juego Picas y Famas
    /// </summary>
    public static class InfrastructureServicesRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            /* Database Context */
            services.AddDatabaseContext(configuration);

            /* Repositories */
            services.AddScoped<IPlayerRepository, PlayerRepository>();
            services.AddScoped<IGameRepository, GameRepository>();
            services.AddScoped<IAttemptRepository, AttemptRepository>();

            return services;
        }

        private static IServiceCollection AddDatabaseContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            services.AddDbContext<GameDbContext>((serviceProvider, options) =>
            {
                options.UseSqlServer(connectionString);

                // Habilitar logging detallado en desarrollo
                var logger = serviceProvider.GetService<ILogger<GameDbContext>>();
                if (logger != null)
                {
                    options.LogTo(message => logger.LogInformation(message));
                }
            });

            return services;
        }
    }
}