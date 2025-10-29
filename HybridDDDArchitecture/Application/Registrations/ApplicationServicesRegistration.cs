using Application.Services;
using Core.Application;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application.Registrations
{
    /// <summary>
    /// Registro de servicios de la capa de aplicación para el juego Picas y Famas
    /// </summary>
    public static class ApplicationServicesRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            /* Automapper */
            services.AddAutoMapper(config => config.AddMaps(Assembly.GetExecutingAssembly()));

            /* MediatR - Command/Query Bus */
            services.AddMediatR(config => config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            services.AddScoped<ICommandQueryBus, MediatrCommandQueryBus>();

            /* Application Services */
            services.AddScoped<ISecretNumberGenerator, SecretNumberGenerator>();
            services.AddScoped<IGuessValidator, GuessValidator>();

            return services;
        }
    }
}
