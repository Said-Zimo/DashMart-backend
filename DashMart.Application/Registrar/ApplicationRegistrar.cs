
using FluentValidation;
using DashMart.Application.Behaviors;
using Microsoft.Extensions.DependencyInjection;

namespace DashMart.Application.Registrar
{
    public static class ApplicationRegistrar
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(cfr =>
            {
                cfr.RegisterServicesFromAssembly(typeof(ApplicationRegistrar).Assembly);

                cfr.AddOpenBehavior(typeof(ValidationBehavior<,>));            
            }
            );

            services.AddValidatorsFromAssembly(typeof(ApplicationRegistrar).Assembly);

            return services;
            
        }
    }
}
