namespace CSharpApp.Infrastructure.Configuration;

public static class HttpConfiguration
{
    public static IServiceCollection AddHttpConfiguration(this IServiceCollection services)
    {
        services.AddHttpClient("Client", (sp, client) =>
        {
            var apiSettings = sp.GetRequiredService<IOptions<RestApiSettings>>().Value;
            client.BaseAddress = new Uri(apiSettings.BaseUrl!);
        })
        .AddPolicyHandler((sp, _) => AddRetryPolicy(sp))
        .ConfigurePrimaryHttpMessageHandler(AddSocketsHandle);

        return services;
    }

    private static IAsyncPolicy<HttpResponseMessage> AddRetryPolicy(IServiceProvider sp)
    {
        var settings = sp.GetRequiredService<IOptions<HttpClientSettings>>().Value;
        return HttpPolicyExtensions.HandleTransientHttpError()
            .WaitAndRetryAsync(settings.RetryCount, retryAttempt => TimeSpan.FromMilliseconds(settings.SleepDuration));
    }

    private static SocketsHttpHandler AddSocketsHandle(IServiceProvider sp)
    {
        var settings = sp.GetRequiredService<IOptions<HttpClientSettings>>().Value;
        var handler = new SocketsHttpHandler
        {
            PooledConnectionLifetime = TimeSpan.FromMinutes(settings.LifeTime)
        };
        return handler;
    }
}