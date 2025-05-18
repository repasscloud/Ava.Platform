using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Ava.Shared.Migrations
{
    /// <inheritdoc />
    public partial class initDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AircraftTypeDesignators",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ICAOCode = table.Column<string>(type: "text", nullable: true),
                    IATATypeCode = table.Column<string>(type: "text", nullable: true),
                    Model = table.Column<string>(type: "text", nullable: false),
                    WikipediaLink = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AircraftTypeDesignators", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AmadeusOAuthTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TokenType = table.Column<string>(type: "text", nullable: false),
                    AccessToken = table.Column<string>(type: "text", nullable: false),
                    ExpiresIn = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AmadeusOAuthTokens", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AvaAIAppJwtTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    JwtToken = table.Column<string>(type: "text", nullable: false),
                    Expires = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsValid = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AvaAIAppJwtTokens", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AvaClientLicenses",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(22)", maxLength: 22, nullable: false),
                    ClientID = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    AppID = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Signature = table.Column<string>(type: "text", nullable: true),
                    SpendThreshold = table.Column<decimal>(type: "numeric", nullable: false),
                    IssuedBy = table.Column<string>(type: "text", nullable: false),
                    GeneratedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AvaClientLicenses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AvaEmployees",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    PrivateKey = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    EmployeeType = table.Column<string>(type: "varchar(20)", nullable: false),
                    Role = table.Column<int>(type: "integer", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    VerificationToken = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AvaEmployees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AvaJwtTokenResponses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    JwtToken = table.Column<string>(type: "text", nullable: false),
                    Expires = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsValid = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AvaJwtTokenResponses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AvaSystemLogs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Level = table.Column<string>(type: "text", nullable: false),
                    Message = table.Column<string>(type: "text", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AvaSystemLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FlightOfferSearchRequestDTOs",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ClientId = table.Column<string>(type: "text", nullable: false),
                    CustomerId = table.Column<string>(type: "text", nullable: false),
                    TravelPolicyId = table.Column<string>(type: "text", nullable: true),
                    OriginLocationCode = table.Column<string>(type: "text", nullable: false),
                    DestinationLocationCode = table.Column<string>(type: "text", nullable: false),
                    IsOneWay = table.Column<bool>(type: "boolean", nullable: false),
                    DepartureDate = table.Column<string>(type: "text", nullable: false),
                    DepartureDateReturn = table.Column<string>(type: "text", nullable: true),
                    Adults = table.Column<int>(type: "integer", nullable: false),
                    CabinClass = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlightOfferSearchRequestDTOs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FlightSearchCriterias",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    _clientId = table.Column<string>(type: "text", nullable: false),
                    _customerId = table.Column<string>(type: "text", nullable: false),
                    _createdAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OriginLocationCode = table.Column<string>(type: "text", nullable: false),
                    DestinationLocationCode = table.Column<string>(type: "text", nullable: false),
                    DepartureDate = table.Column<string>(type: "text", nullable: false),
                    ReturnDate = table.Column<string>(type: "text", nullable: true),
                    Adults = table.Column<int>(type: "integer", nullable: false),
                    Children = table.Column<int>(type: "integer", nullable: true),
                    Infants = table.Column<int>(type: "integer", nullable: true),
                    TravelClass = table.Column<int>(type: "integer", nullable: false),
                    IncludedAirlineCodes = table.Column<string>(type: "text", nullable: true),
                    ExcludedAirlineCodes = table.Column<string>(type: "text", nullable: true),
                    NonStop = table.Column<bool>(type: "boolean", nullable: false),
                    CurrencyCode = table.Column<string>(type: "text", nullable: true),
                    MaxPrice = table.Column<int>(type: "integer", nullable: true),
                    MaxResultCount = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlightSearchCriterias", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GitHubRepoOAuthTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Token = table.Column<string>(type: "text", nullable: false),
                    Owner = table.Column<string>(type: "text", nullable: false),
                    Repo = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GitHubRepoOAuthTokens", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IATAAirportCodes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Identity = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Latitude = table.Column<decimal>(type: "numeric", nullable: false),
                    Longitude = table.Column<decimal>(type: "numeric", nullable: false),
                    ElevationFt = table.Column<int>(type: "integer", nullable: true),
                    Continent = table.Column<string>(type: "text", nullable: false),
                    IsoCountry = table.Column<string>(type: "text", nullable: false),
                    IsoRegion = table.Column<string>(type: "text", nullable: false),
                    Municipality = table.Column<string>(type: "text", nullable: true),
                    ScheduledService = table.Column<string>(type: "text", nullable: false),
                    ICAOCode = table.Column<string>(type: "text", nullable: true),
                    IATACode = table.Column<string>(type: "text", nullable: true),
                    GPSCode = table.Column<string>(type: "text", nullable: true),
                    LocalCode = table.Column<string>(type: "text", nullable: true),
                    HomeLink = table.Column<string>(type: "text", nullable: true),
                    WikipediaLink = table.Column<string>(type: "text", nullable: true),
                    Keywords = table.Column<string>(type: "text", nullable: true),
                    _Region = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IATAAirportCodes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LateFeeConfigs",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    LicenseAgreementId = table.Column<string>(type: "character varying(14)", maxLength: 14, nullable: false),
                    GracePeriodDays = table.Column<int>(type: "integer", nullable: false),
                    UseFixedAmount = table.Column<bool>(type: "boolean", nullable: false),
                    FixedAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    PercentOfInvoice = table.Column<decimal>(type: "numeric(6,4)", precision: 6, scale: 4, nullable: false),
                    RecurringOption = table.Column<int>(type: "integer", nullable: false),
                    MaxLateFeeCap = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LateFeeConfigs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LicenseAgreements",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(14)", maxLength: 14, nullable: false),
                    AvaClientId = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    PaymentTerms = table.Column<int>(type: "integer", nullable: false),
                    PaymentMethod = table.Column<int>(type: "integer", nullable: false),
                    BillingType = table.Column<int>(type: "integer", nullable: false),
                    BillingFrequency = table.Column<int>(type: "integer", nullable: false),
                    RemittanceEmail = table.Column<string>(type: "text", nullable: false),
                    AutoRenew = table.Column<bool>(type: "boolean", nullable: false),
                    GracePeriodDays = table.Column<int>(type: "integer", nullable: false),
                    AccessFee = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    PrepaidBalance = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    PaymentStatus = table.Column<int>(type: "integer", nullable: false),
                    LateFeeConfigId = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    AccountThreshold = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    TaxRate = table.Column<decimal>(type: "numeric(6,4)", precision: 6, scale: 4, nullable: false),
                    TrialEndsOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Discount = table.Column<decimal>(type: "numeric(5,4)", precision: 5, scale: 4, nullable: false),
                    DiscountExpires = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    PnrCreationFee = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    PnrChangeFee = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    FlightMarkupPercent = table.Column<decimal>(type: "numeric(5,4)", precision: 5, scale: 4, nullable: false),
                    FlightPerItemFee = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    FlightFeeType = table.Column<int>(type: "integer", nullable: false),
                    HotelMarkupPercent = table.Column<decimal>(type: "numeric(5,4)", precision: 5, scale: 4, nullable: false),
                    HotelPerItemFee = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    HotelFeeType = table.Column<int>(type: "integer", nullable: false),
                    CarMarkupPercent = table.Column<decimal>(type: "numeric(5,4)", precision: 5, scale: 4, nullable: false),
                    CarPerItemFee = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    CarFeeType = table.Column<int>(type: "integer", nullable: false),
                    RailMarkupPercent = table.Column<decimal>(type: "numeric(5,4)", precision: 5, scale: 4, nullable: false),
                    RailPerItemFee = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    RailFeeType = table.Column<int>(type: "integer", nullable: false),
                    TransferMarkupPercent = table.Column<decimal>(type: "numeric(5,4)", precision: 5, scale: 4, nullable: false),
                    TransferPerItemFee = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    TransferFeeType = table.Column<int>(type: "integer", nullable: false),
                    ActivityMarkupPercent = table.Column<decimal>(type: "numeric(5,4)", precision: 5, scale: 4, nullable: false),
                    ActivityPerItemFee = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    ActivityFeeType = table.Column<int>(type: "integer", nullable: false),
                    TravelMarkupPercent = table.Column<decimal>(type: "numeric(5,4)", precision: 5, scale: 4, nullable: false),
                    TravelPerItemFee = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    TravelFeeType = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LicenseAgreements", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "QantasCustomBookingRequest",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Reference = table.Column<string>(type: "text", nullable: false),
                    ClientCode = table.Column<string>(type: "text", nullable: false),
                    EmployeeCode = table.Column<string>(type: "text", nullable: false),
                    PassengerCount = table.Column<int>(type: "integer", nullable: false),
                    IsOneWay = table.Column<bool>(type: "boolean", nullable: false),
                    DepartCity = table.Column<string>(type: "text", nullable: false),
                    DestinationCity = table.Column<string>(type: "text", nullable: false),
                    Date = table.Column<string>(type: "text", nullable: false),
                    ReturnDate = table.Column<string>(type: "text", nullable: true),
                    _fareClass = table.Column<string>(type: "text", nullable: false),
                    _includeJetstar = table.Column<bool>(type: "boolean", nullable: false),
                    _currencyCode = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QantasCustomBookingRequest", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "QantasCustomIATACodes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    IATACode = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QantasCustomIATACodes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Regions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Regions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StorageEntries",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    SerializedData = table.Column<string>(type: "text", nullable: false),
                    Expires = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StorageEntries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TPCityIATACodes",
                columns: table => new
                {
                    Code = table.Column<string>(type: "text", nullable: false),
                    NameTranslations_En = table.Column<string>(type: "text", nullable: false),
                    CityCode = table.Column<string>(type: "text", nullable: true),
                    CountryCode = table.Column<string>(type: "text", nullable: true),
                    TimeZone = table.Column<string>(type: "text", nullable: false),
                    IataType = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Coordinates_Lat = table.Column<double>(type: "double precision", nullable: false),
                    Coordinates_Lon = table.Column<double>(type: "double precision", nullable: false),
                    Flightable = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TPCityIATACodes", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "TravelSearchRecords",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SearchId = table.Column<string>(type: "text", nullable: false),
                    TravelType = table.Column<int>(type: "integer", nullable: false),
                    FlightSubComponent = table.Column<int>(type: "integer", nullable: false),
                    HotelSubComponent = table.Column<int>(type: "integer", nullable: false),
                    CarSubComponent = table.Column<int>(type: "integer", nullable: false),
                    RailSubComponent = table.Column<int>(type: "integer", nullable: false),
                    TransferSubComponent = table.Column<int>(type: "integer", nullable: false),
                    ActivitySubComponent = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Payload = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TravelSearchRecords", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    ProviderKey = table.Column<string>(type: "text", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    RoleId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AvaSalesRecords",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    AvaEmployeeRecordId = table.Column<string>(type: "text", nullable: false),
                    SalesPerson = table.Column<string>(type: "text", nullable: false),
                    LicenseId = table.Column<string>(type: "character varying(22)", nullable: true),
                    ClientId = table.Column<string>(type: "text", nullable: true),
                    PaymentMethod = table.Column<string>(type: "text", nullable: false),
                    PaymentReference = table.Column<string>(type: "text", nullable: false),
                    SalesDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AvaSalesRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AvaSalesRecords_AvaClientLicenses_LicenseId",
                        column: x => x.LicenseId,
                        principalTable: "AvaClientLicenses",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Continents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    IsoCode = table.Column<string>(type: "text", nullable: false),
                    RegionId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Continents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Continents_Regions_RegionId",
                        column: x => x.RegionId,
                        principalTable: "Regions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    IsoCode = table.Column<string>(type: "text", nullable: false),
                    Flag = table.Column<string>(type: "text", nullable: false),
                    ContinentId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Countries_Continents_ContinentId",
                        column: x => x.ContinentId,
                        principalTable: "Continents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AvaClients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ClientId = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    CompanyName = table.Column<string>(type: "text", nullable: false),
                    TaxIdType = table.Column<string>(type: "text", nullable: true),
                    TaxId = table.Column<string>(type: "text", nullable: true),
                    TaxLastValidated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastUpdated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    AddressLine1 = table.Column<string>(type: "text", nullable: true),
                    AddressLine2 = table.Column<string>(type: "text", nullable: true),
                    AddressLine3 = table.Column<string>(type: "text", nullable: true),
                    City = table.Column<string>(type: "text", nullable: true),
                    State = table.Column<string>(type: "text", nullable: true),
                    PostalCode = table.Column<string>(type: "text", nullable: true),
                    Country = table.Column<string>(type: "text", nullable: true),
                    ContactPersonFirstName = table.Column<string>(type: "text", nullable: false),
                    ContactPersonLastName = table.Column<string>(type: "text", nullable: false),
                    ContactPersonCountryCode = table.Column<string>(type: "text", nullable: false),
                    ContactPersonPhone = table.Column<string>(type: "text", nullable: false),
                    ContactPersonEmail = table.Column<string>(type: "text", nullable: false),
                    ContactPersonJobTitle = table.Column<string>(type: "text", nullable: true),
                    BillingPersonFirstName = table.Column<string>(type: "text", nullable: true),
                    BillingPersonLastName = table.Column<string>(type: "text", nullable: true),
                    BillingPersonCountryCode = table.Column<string>(type: "text", nullable: true),
                    BillingPersonPhone = table.Column<string>(type: "text", nullable: true),
                    BillingPersonEmail = table.Column<string>(type: "text", nullable: true),
                    BillingPersonJobTitle = table.Column<string>(type: "text", nullable: true),
                    AdminPersonFirstName = table.Column<string>(type: "text", nullable: true),
                    AdminPersonLastName = table.Column<string>(type: "text", nullable: true),
                    AdminPersonCountryCode = table.Column<string>(type: "text", nullable: true),
                    AdminPersonPhone = table.Column<string>(type: "text", nullable: true),
                    AdminPersonEmail = table.Column<string>(type: "text", nullable: true),
                    AdminPersonJobTitle = table.Column<string>(type: "text", nullable: true),
                    DefaultCurrency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    LicenseAgreementId = table.Column<string>(type: "text", nullable: true),
                    DefaultTravelPolicyId = table.Column<string>(type: "character varying(14)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AvaClients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AvaClientSupportedDomains",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SupportedEmailDomain = table.Column<string>(type: "character varying(253)", maxLength: 253, nullable: false),
                    AvaClientId = table.Column<int>(type: "integer", nullable: false),
                    ClientCode = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AvaClientSupportedDomains", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AvaClientSupportedDomains_AvaClients_AvaClientId",
                        column: x => x.AvaClientId,
                        principalTable: "AvaClients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TravelPolicies",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(14)", maxLength: 14, nullable: false),
                    PolicyName = table.Column<string>(type: "text", nullable: false),
                    AvaClientId = table.Column<int>(type: "integer", nullable: false),
                    DefaultCurrencyCode = table.Column<string>(type: "text", nullable: false),
                    MaxFlightPrice = table.Column<int>(type: "integer", nullable: false),
                    DefaultFlightSeating = table.Column<string>(type: "text", nullable: false),
                    MaxFlightSeating = table.Column<string>(type: "text", nullable: false),
                    IncludedAirlineCodes = table.Column<string>(type: "text", nullable: true),
                    ExcludedAirlineCodes = table.Column<string>(type: "text", nullable: true),
                    CabinClassCoverage = table.Column<string>(type: "text", nullable: false),
                    NonStopFlight = table.Column<bool>(type: "boolean", nullable: false),
                    FlightBookingTimeAvailableFrom = table.Column<string>(type: "text", nullable: true),
                    FlightBookingTimeAvailableTo = table.Column<string>(type: "text", nullable: true),
                    EnableSaturdayFlightBookings = table.Column<bool>(type: "boolean", nullable: false),
                    EnableSundayFlightBookings = table.Column<bool>(type: "boolean", nullable: false),
                    DefaultCalendarDaysInAdvanceForFlightBooking = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TravelPolicies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TravelPolicies_AvaClients_AvaClientId",
                        column: x => x.AvaClientId,
                        principalTable: "AvaClients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AvaUsers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AspNetUsersId = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    MiddleName = table.Column<string>(type: "text", nullable: true),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    OriginLocationCode = table.Column<string>(type: "text", nullable: true),
                    DefaultFlightSeating = table.Column<string>(type: "text", nullable: false),
                    MaxFlightSeating = table.Column<string>(type: "text", nullable: false),
                    IncludedAirlineCodes = table.Column<string>(type: "text", nullable: true),
                    ExcludedAirlineCodes = table.Column<string>(type: "text", nullable: true),
                    NonStopFlight = table.Column<bool>(type: "boolean", nullable: false),
                    DefaultCurrencyCode = table.Column<string>(type: "text", nullable: false),
                    MaxFlightPrice = table.Column<int>(type: "integer", nullable: false),
                    TravelPolicyId = table.Column<string>(type: "character varying(14)", nullable: true),
                    AvaClientId = table.Column<int>(type: "integer", nullable: true),
                    ClientId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AvaUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AvaUsers_AvaClients_AvaClientId",
                        column: x => x.AvaClientId,
                        principalTable: "AvaClients",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AvaUsers_TravelPolicies_TravelPolicyId",
                        column: x => x.TravelPolicyId,
                        principalTable: "TravelPolicies",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AvaUserSysPreferences",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AspNetUsersId = table.Column<string>(type: "text", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    MiddleName = table.Column<string>(type: "text", nullable: true),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    OriginLocationCode = table.Column<string>(type: "text", nullable: true),
                    DefaultFlightSeating = table.Column<string>(type: "text", nullable: false),
                    MaxFlightSeating = table.Column<string>(type: "text", nullable: false),
                    IncludedAirlineCodes = table.Column<string>(type: "text", nullable: true),
                    ExcludedAirlineCodes = table.Column<string>(type: "text", nullable: true),
                    CabinClassCoverage = table.Column<string>(type: "text", nullable: false),
                    NonStopFlight = table.Column<bool>(type: "boolean", nullable: false),
                    DefaultCurrencyCode = table.Column<string>(type: "text", nullable: false),
                    MaxFlightPrice = table.Column<int>(type: "integer", nullable: false),
                    MaxResults = table.Column<int>(type: "integer", nullable: false),
                    FlightBookingTimeAvailableFrom = table.Column<string>(type: "text", nullable: true),
                    FlightBookingTimeAvailableTo = table.Column<string>(type: "text", nullable: true),
                    EnableSaturdayFlightBookings = table.Column<bool>(type: "boolean", nullable: false),
                    EnableSundayFlightBookings = table.Column<bool>(type: "boolean", nullable: false),
                    DefaultCalendarDaysInAdvanceForFlightBooking = table.Column<int>(type: "integer", nullable: true),
                    TravelPolicyName = table.Column<string>(type: "text", nullable: true),
                    TravelPolicyId = table.Column<string>(type: "character varying(14)", nullable: true),
                    AvaClientId = table.Column<int>(type: "integer", nullable: true),
                    ClientId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AvaUserSysPreferences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AvaUserSysPreferences_AvaClients_AvaClientId",
                        column: x => x.AvaClientId,
                        principalTable: "AvaClients",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AvaUserSysPreferences_TravelPolicies_TravelPolicyId",
                        column: x => x.TravelPolicyId,
                        principalTable: "TravelPolicies",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ContinentTravelPolicy",
                columns: table => new
                {
                    ContinentsId = table.Column<int>(type: "integer", nullable: false),
                    TravelPolicyId = table.Column<string>(type: "character varying(14)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContinentTravelPolicy", x => new { x.ContinentsId, x.TravelPolicyId });
                    table.ForeignKey(
                        name: "FK_ContinentTravelPolicy_Continents_ContinentsId",
                        column: x => x.ContinentsId,
                        principalTable: "Continents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContinentTravelPolicy_TravelPolicies_TravelPolicyId",
                        column: x => x.TravelPolicyId,
                        principalTable: "TravelPolicies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CountryTravelPolicy",
                columns: table => new
                {
                    CountriesId = table.Column<int>(type: "integer", nullable: false),
                    TravelPolicyId = table.Column<string>(type: "character varying(14)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CountryTravelPolicy", x => new { x.CountriesId, x.TravelPolicyId });
                    table.ForeignKey(
                        name: "FK_CountryTravelPolicy_Countries_CountriesId",
                        column: x => x.CountriesId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CountryTravelPolicy_TravelPolicies_TravelPolicyId",
                        column: x => x.TravelPolicyId,
                        principalTable: "TravelPolicies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RegionTravelPolicy",
                columns: table => new
                {
                    RegionsId = table.Column<int>(type: "integer", nullable: false),
                    TravelPolicyId = table.Column<string>(type: "character varying(14)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegionTravelPolicy", x => new { x.RegionsId, x.TravelPolicyId });
                    table.ForeignKey(
                        name: "FK_RegionTravelPolicy_Regions_RegionsId",
                        column: x => x.RegionsId,
                        principalTable: "Regions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RegionTravelPolicy_TravelPolicies_TravelPolicyId",
                        column: x => x.TravelPolicyId,
                        principalTable: "TravelPolicies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TravelPolicyDisabledCountries",
                columns: table => new
                {
                    TravelPolicyId = table.Column<string>(type: "character varying(14)", nullable: false),
                    CountryId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TravelPolicyDisabledCountries", x => new { x.TravelPolicyId, x.CountryId });
                    table.ForeignKey(
                        name: "FK_TravelPolicyDisabledCountries_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TravelPolicyDisabledCountries_TravelPolicies_TravelPolicyId",
                        column: x => x.TravelPolicyId,
                        principalTable: "TravelPolicies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AvaClients_DefaultTravelPolicyId",
                table: "AvaClients",
                column: "DefaultTravelPolicyId");

            migrationBuilder.CreateIndex(
                name: "IX_AvaClientSupportedDomains_AvaClientId",
                table: "AvaClientSupportedDomains",
                column: "AvaClientId");

            migrationBuilder.CreateIndex(
                name: "IX_AvaClientSupportedDomains_SupportedEmailDomain",
                table: "AvaClientSupportedDomains",
                column: "SupportedEmailDomain",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AvaSalesRecords_LicenseId",
                table: "AvaSalesRecords",
                column: "LicenseId");

            migrationBuilder.CreateIndex(
                name: "IX_AvaUsers_AspNetUsersId",
                table: "AvaUsers",
                column: "AspNetUsersId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AvaUsers_AvaClientId",
                table: "AvaUsers",
                column: "AvaClientId");

            migrationBuilder.CreateIndex(
                name: "IX_AvaUsers_Email",
                table: "AvaUsers",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AvaUsers_TravelPolicyId",
                table: "AvaUsers",
                column: "TravelPolicyId");

            migrationBuilder.CreateIndex(
                name: "IX_AvaUserSysPreferences_AvaClientId",
                table: "AvaUserSysPreferences",
                column: "AvaClientId");

            migrationBuilder.CreateIndex(
                name: "IX_AvaUserSysPreferences_TravelPolicyId",
                table: "AvaUserSysPreferences",
                column: "TravelPolicyId");

            migrationBuilder.CreateIndex(
                name: "IX_Continents_IsoCode",
                table: "Continents",
                column: "IsoCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Continents_Name",
                table: "Continents",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Continents_RegionId",
                table: "Continents",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_ContinentTravelPolicy_TravelPolicyId",
                table: "ContinentTravelPolicy",
                column: "TravelPolicyId");

            migrationBuilder.CreateIndex(
                name: "IX_Countries_ContinentId",
                table: "Countries",
                column: "ContinentId");

            migrationBuilder.CreateIndex(
                name: "IX_Countries_IsoCode",
                table: "Countries",
                column: "IsoCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Countries_Name",
                table: "Countries",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CountryTravelPolicy_TravelPolicyId",
                table: "CountryTravelPolicy",
                column: "TravelPolicyId");

            migrationBuilder.CreateIndex(
                name: "IX_Regions_Name",
                table: "Regions",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RegionTravelPolicy_TravelPolicyId",
                table: "RegionTravelPolicy",
                column: "TravelPolicyId");

            migrationBuilder.CreateIndex(
                name: "IX_TravelPolicies_AvaClientId",
                table: "TravelPolicies",
                column: "AvaClientId");

            migrationBuilder.CreateIndex(
                name: "IX_TravelPolicyDisabledCountries_CountryId",
                table: "TravelPolicyDisabledCountries",
                column: "CountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_AvaClients_TravelPolicies_DefaultTravelPolicyId",
                table: "AvaClients",
                column: "DefaultTravelPolicyId",
                principalTable: "TravelPolicies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AvaClients_TravelPolicies_DefaultTravelPolicyId",
                table: "AvaClients");

            migrationBuilder.DropTable(
                name: "AircraftTypeDesignators");

            migrationBuilder.DropTable(
                name: "AmadeusOAuthTokens");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "AvaAIAppJwtTokens");

            migrationBuilder.DropTable(
                name: "AvaClientSupportedDomains");

            migrationBuilder.DropTable(
                name: "AvaEmployees");

            migrationBuilder.DropTable(
                name: "AvaJwtTokenResponses");

            migrationBuilder.DropTable(
                name: "AvaSalesRecords");

            migrationBuilder.DropTable(
                name: "AvaSystemLogs");

            migrationBuilder.DropTable(
                name: "AvaUsers");

            migrationBuilder.DropTable(
                name: "AvaUserSysPreferences");

            migrationBuilder.DropTable(
                name: "ContinentTravelPolicy");

            migrationBuilder.DropTable(
                name: "CountryTravelPolicy");

            migrationBuilder.DropTable(
                name: "FlightOfferSearchRequestDTOs");

            migrationBuilder.DropTable(
                name: "FlightSearchCriterias");

            migrationBuilder.DropTable(
                name: "GitHubRepoOAuthTokens");

            migrationBuilder.DropTable(
                name: "IATAAirportCodes");

            migrationBuilder.DropTable(
                name: "LateFeeConfigs");

            migrationBuilder.DropTable(
                name: "LicenseAgreements");

            migrationBuilder.DropTable(
                name: "QantasCustomBookingRequest");

            migrationBuilder.DropTable(
                name: "QantasCustomIATACodes");

            migrationBuilder.DropTable(
                name: "RegionTravelPolicy");

            migrationBuilder.DropTable(
                name: "StorageEntries");

            migrationBuilder.DropTable(
                name: "TPCityIATACodes");

            migrationBuilder.DropTable(
                name: "TravelPolicyDisabledCountries");

            migrationBuilder.DropTable(
                name: "TravelSearchRecords");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "AvaClientLicenses");

            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropTable(
                name: "Continents");

            migrationBuilder.DropTable(
                name: "Regions");

            migrationBuilder.DropTable(
                name: "TravelPolicies");

            migrationBuilder.DropTable(
                name: "AvaClients");
        }
    }
}
