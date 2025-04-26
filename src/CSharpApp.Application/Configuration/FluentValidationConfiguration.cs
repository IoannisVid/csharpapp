namespace CSharpApp.Application.Configuration
{
    public static class FluentValidationConfiguration
    {
        public static IServiceCollection AddFluentValidationConfiguration(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(typeof(FluentValidationConfiguration).Assembly);
            return services;
        }
    }
}
