namespace Ava.Shared.DependencyInjection;

public static class ServiceRegistrationExtensions
{
    public static IServiceCollection AddAvaSharedServices(this IServiceCollection services, IConfiguration config)
    {
        var connectionString = config.GetConnectionString("PostgresConnection")
            ?? throw new InvalidOperationException("Connection string 'PostgresConnection' not found.");

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<ILoggerService, LoggerService>();

        // Object Storage Service
        services.AddScoped<IStorageService, StorageService>();
        
        // Ava API Service
        services.AddScoped<IAvaApiService, AvaApiService>();
        
        // JWT Token Service
        services.AddScoped<IJwtTokenService, JwtTokenService>();

        // Authentication Info Service
        //services.AddScoped<IAuthenticationInfoService, AuthenticationInfoService>();

        //services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

        // User Pref Service
        //services.AddScoped<IAvaUserSysPrefService, AvaUserSysPrefService>();

        // Add Blazored.LocalStorage (for cookies and stuff)
        //services.AddBlazoredLocalStorage();

        return services;
    }
}
