namespace Ava.Shared.DependencyInjection;

public static class ServiceRegistrationExtensions
{

    // Ava.Shared.DependencyInjection.ServiceRegistrationExtensions.cs
    public static IServiceCollection AddAvaSharedServices(this IServiceCollection services, IConfiguration config, bool includeWebOnly = false)
    {
        var connectionString = config.GetConnectionString("PostgresConnection")
            ?? throw new InvalidOperationException("Connection string 'PostgresConnection' not found.");

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<ILoggerService, LoggerService>();
        services.AddScoped<IStorageService, StorageService>();  // Object Storage Service
        services.AddScoped<IAvaApiService, AvaApiService>();  // Ava API Service
        services.AddScoped<IJwtTokenService, JwtTokenService>();  // JWT Token Service

        // ✅ Only add this when includeWebOnly is true
        if (includeWebOnly)
        {
            services.AddScoped<IAvaUserSysPrefService, AvaUserSysPrefService>();  // User Pref Service
        }

        //services.AddScoped<IAuthenticationInfoService, AuthenticationInfoService>();  // Authentication Info Service
        //services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();
        //services.AddBlazoredLocalStorage();  // Add Blazored.LocalStorage (for cookies and stuff)


        return services;
    }
}
