using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RentM.Application.Interfaces;
using RentM.Application.Services;
using RentM.Infrastructure.Data;
using RentM.Infrastructure.Interfaces;
using RentM.Infrastructure.Messaging;
using RentM.Infrastructure.Repositories;

namespace RentM.API.Extensions
{
    public static class ServiceColletionExtension
    {
        public static IServiceCollection AddDependencyInjection(this IServiceCollection services, IConfiguration configuration) // Add IConfiguration parameter
        {
            var rabbitMqConfig = configuration.GetSection("RabbitMQ");
            var hostName = rabbitMqConfig["HostName"];
            var queueName = rabbitMqConfig["QueueName"];

            // Registrar Publisher e Subscriber
            services.AddScoped<IMotorcycleEventRepository, MotorcycleEventRepository>();
            services.AddTransient<MotorcycleEventPublisher>(provider =>
            {
                return new MotorcycleEventPublisher(hostName, queueName);
            });
            services.AddSingleton<MotorcycleEventSubscriber>(provider =>
            {
                var serviceProvider = provider.GetRequiredService<IServiceProvider>();
                return new MotorcycleEventSubscriber(hostName, queueName, serviceProvider);
            });

            // Application Layer
            services.AddScoped<IMotorcycleService, MotorcycleService>();
            services.AddScoped<IDeliveryPersonService, DeliveryPersonService>();
            services.AddScoped<IRentalService, RentalService>();

            // Infrastructure Layer
            services.AddScoped<IMotorcycleRepository, MotorcycleRepository>();
            services.AddScoped<IDeliveryPersonRepository, DeliveryPersonRepository>();
            services.AddScoped<IRentalRepository, RentalRepository>();

            // Add DbContext with proper configuration
            services.AddDbContext<RentMDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("PostgreSqlConnection")));

            return services;
        }
    }
}
