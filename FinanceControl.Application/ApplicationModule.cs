using FinanceControl.Application.Common;
using FinanceControl.Application.UseCases.Accounts.CreateAccount;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FinanceControl.Application;

public static class ApplicationModule
{
    public static IServiceCollection AddAplication(this IServiceCollection services)
    {
        services
            .AddHandlers()
            .AddValidation()
            .AddNotifications();
        return services;
    }
    
    private static IServiceCollection AddHandlers(this IServiceCollection services)
    {
        services.AddMediatR(config =>
            config.RegisterServicesFromAssemblyContaining<CreateAccountHandler>());
        
        return services;
    }
    
    private static IServiceCollection AddValidation(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<CreateAccountValidator>(ServiceLifetime.Singleton);

        return services;
    }
    
    private static IServiceCollection AddNotifications(this IServiceCollection services)
    {
        services.AddScoped<NotificationContext>();
        return services;
    }
}