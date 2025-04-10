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

        // AvaAPI - for internal API communication
        builder.Services.AddHttpClient("AvaAPI", client =>
        {
            client.BaseAddress = new Uri("http://ava-api:5165/"); // Replace with actual API URL
            client.Timeout = TimeSpan.FromSeconds(30);
        });

        // Github CDN - for external CDN calls to Github
        builder.Services.AddHttpClient("GithubCDN", client =>
        {
            client.BaseAddress = new Uri("https://raw.githubusercontent.com/");
            client.Timeout = TimeSpan.FromSeconds(30);
        });

        // Add services to the container.
        builder.Services.AddAvaSharedServices(builder.Configuration, includeWebOnly: true);

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

        // Ava API Service
        builder.Services.AddScoped<IAvaApiService, AvaApiService>();
        // Authentication Info Service
        builder.Services.AddScoped<IAuthenticationInfoService, AuthenticationInfoService>();
        builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();
        builder.Services.AddScoped<IGithubCDNService, GithubCDNService>();
        // Add Blazored.LocalStorage (for cookies and stuff)
        builder.Services.AddBlazoredLocalStorage();

        var app = builder.Build();

        // 🌏 Set Australian culture (en-AU)
        var defaultCulture = new CultureInfo("en-AU");
        var localizationOptions = new RequestLocalizationOptions
        {
            DefaultRequestCulture = new RequestCulture(defaultCulture),
            SupportedCultures = new[] { defaultCulture },
            SupportedUICultures = new[] { defaultCulture }
        };
        app.UseRequestLocalization(localizationOptions);

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
