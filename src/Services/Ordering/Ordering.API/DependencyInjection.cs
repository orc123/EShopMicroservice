namespace Ordering.API;

public static class DependencyInjection
{
    public static IServiceCollection AddApiServices
        (this IServiceCollection services)
    {
        return services;
    }

    public static WebApplication UseApiService(this WebApplication app)
    {
        return app;
    }
}