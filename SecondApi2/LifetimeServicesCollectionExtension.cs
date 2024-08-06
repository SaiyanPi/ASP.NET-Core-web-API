using SecondApi2.Services;

namespace SecondApi2;

//  This is an extension method for the IServiceCollection interface.
//  It is used to register all services at once in the program file
//  this way program file will be smaller and easier to read.
public static class LifetimeServicesCollectionExtensions
{
    public static IServiceCollection AddLifetimeServices(this IServiceCollection services)
    {
        services.AddScoped<IScopedService, ScopedService>();
        services.AddTransient<ITransientService, TransientService>();
        services.AddSingleton<ISingletonService, SingletonService>();

        return services;
    }
}