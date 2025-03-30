namespace Ava.WebApp;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Stolen from webapi
        builder.Services.AddSingleton(new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true,
        });

        // Add MudBlazor services
        builder.Services.AddMudServices();

        // Add CORS
        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder.WithOrigins("https://localhost:7064")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });

        // Register HttpClient for Blazor Server
        builder.Services.AddHttpClient("AvaAPI", client =>
        {
            client.BaseAddress = new Uri("http://ava-api:5165/"); // Replace with actual API URL
            client.Timeout = TimeSpan.FromSeconds(30);
        });

        // Add services to the container.
        builder.Services.AddAvaSharedServices(builder.Configuration);

        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();

        builder.Services.AddCascadingAuthenticationState();
        builder.Services.AddScoped<IdentityUserAccessor>();
        builder.Services.AddScoped<IdentityRedirectManager>();
        builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

        builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = IdentityConstants.ApplicationScheme;
                options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
            })
            .AddMicrosoftAccount(microsoftOptions =>
            {
                microsoftOptions.ClientId = builder.Configuration["Authentication:Microsoft:ClientId"]
                    ?? throw new InvalidOperationException("Authentication:Microsoft:ClientId is missing in the configuration");
                microsoftOptions.ClientSecret = builder.Configuration["Authentication:Microsoft:ClientSecret"]
                    ?? throw new InvalidOperationException("Authentication:Microsoft:ClientSecret is missing in the configuration");
                microsoftOptions.AuthorizationEndpoint = "https://login.microsoftonline.com/consumers/oauth2/v2.0/authorize";
                microsoftOptions.TokenEndpoint = "https://login.microsoftonline.com/consumers/oauth2/v2.0/token";
            })
            .AddIdentityCookies();

        var connectionString = builder.Configuration.GetConnectionString("PostgresConnection") ?? throw new InvalidOperationException("Connection string 'PostgresConnection' not found.");
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connectionString));
            // options.UseSqlite(connectionString));  <-- uses SqliteDB
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddSignInManager()
            .AddDefaultTokenProviders();

        // Register LoggerService (custom) (comes from Ava.API)
        //builder.Services.AddScoped<ILoggerService, LoggerService>();

        // Object Storage Service
        // builder.Services.AddScoped<IStorageService, StorageService>();
        
        // Ava API Service
        // builder.Services.AddScoped<IAvaApiService, AvaApiService>();
        
        // JWT Token Service
        // builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

        // Authentication Info Service
        builder.Services.AddScoped<IAuthenticationInfoService, AuthenticationInfoService>();

        builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();



        // User Pref Service
        builder.Services.AddScoped<IAvaUserSysPrefService, AvaUserSysPrefService>();

        // Add Blazored.LocalStorage (for cookies and stuff)
        builder.Services.AddBlazoredLocalStorage();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseAntiforgery();

        app.MapStaticAssets();
        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();

        // Add additional endpoints required by the Identity /Account Razor components.
        app.MapAdditionalIdentityEndpoints();

        app.Run();
    }
}
