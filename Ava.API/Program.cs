namespace Ava.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add global singleton services here
        builder.Services.AddSingleton<IDurationParserService, DurationParserService>();
        builder.Services.AddSingleton(new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true,
        });

        // Add services to the container.
        builder.Services.AddHttpClient();  // Register HttpClient factory

        builder.Services.AddHealthChecks();

        builder.Services.AddAvaSharedServices(builder.Configuration);

        builder.Services.AddScoped<IAmadeusAuthService, AmadeusAuthService>();  // register Amadeus API service
        builder.Services.AddScoped<IAmadeusFlightSearchService, AmadeusFlightSearchService>();  // for searching Amadeus flight data
        builder.Services.AddScoped<ITPCityIATACodeService, TPCityIATACodeService>();  // register TravelPayouts IATA City Code service
        
        // Register LoggerService (custom)
        builder.Services.AddScoped<ILoggerService, LoggerService>();
        
        // REgister License Generator & Validator
        builder.Services.AddScoped<IAvaLicenseGenerator, AvaLicenseGenerator>();
        builder.Services.AddScoped<IAvaLicenseValidator, AvaLicenseValidator>();

        // Ava Employee Service
        builder.Services.AddScoped<IAvaEmployeeService, AvaEmployeeService>();

        // CustomPasswordHasher
        builder.Services.AddSingleton<ICustomPasswordHasher, CustomPasswordHasher>();

        // GitHub Ticket Service
        builder.Services.Configure<GitHubSettings>(builder.Configuration.GetSection("GitHub"));
        builder.Services.AddHttpClient("GitHubIssuesAPI", client =>
        {
            client.BaseAddress = new Uri("https://api.github.com"); // GitHub base
            client.Timeout = TimeSpan.FromSeconds(30);
        });
        builder.Services.AddScoped<IGitHubTicketService, GitHubTicketService>();


        // Github service
        builder.Services.AddScoped<IGitHubService, GitHubService>();

        builder.Services.AddOptions();
        builder.Services.AddHttpClient<ResendClient>();
        builder.Services.Configure<ResendClientOptions>(options =>
        {
            options.ApiToken = builder.Configuration.GetValue<string>("AvaSettings:ResendKey")
                ?? throw new InvalidOperationException("AvaSettings:ResendKey missing in configuration.");
        });
        builder.Services.AddTransient<IResend, ResendClient>();

        // Register AmadeusUrlBuilder
        builder.Services.AddSingleton<AmadeusUrlBuilder>();

        // configure the Amadeus settings from appsettings.json
        builder.Services.Configure<AmadeusSettings>(
            builder.Configuration.GetSection("Amadeus"));

        // remove the infinite looping that occurs on circular references
        builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });

        // // Configure PostgreSQL Connection
        // var connectionString = builder.Configuration.GetConnectionString("PostgresConnection") ?? throw new InvalidOperationException("Connection string 'PostgresConnection' not found.");
        // builder.Services.AddDbContext<ApplicationDbContext>(options =>
        //     options.UseNpgsql(connectionString));

        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        // Add Swagger services manually
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Ava.API",
                Version = "v1",
                Description = "API documentation for Ava.API",
            });
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();

            app.UseDeveloperExceptionPage();

            // Enable Swagger UI
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ava API v1");
                c.RoutePrefix = "swagger"; // Swagger UI at /swagger
            });
        }

        // enable static files and set the default file to index.html
        app.UseDefaultFiles();
        app.UseStaticFiles();

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        // add a healthcheck
        app.MapHealthChecks("/health");

        app.Run();
    }
}
