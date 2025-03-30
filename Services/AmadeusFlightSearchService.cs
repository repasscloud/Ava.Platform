namespace Ava.API.Services;

public class AmadeusFlightSearchService : IAmadeusFlightSearchService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly IAmadeusAuthService _authService;
    private readonly ILoggerService _loggerService;
    private readonly AmadeusUrlBuilder _urlBuilder;
    private readonly JsonSerializerOptions _jsonOptions;

    public AmadeusFlightSearchService(
        IHttpClientFactory httpClientFactory,
        ApplicationDbContext context,
        IConfiguration configuration,
        IAmadeusAuthService authService,
        ILoggerService loggerService,
        AmadeusUrlBuilder amadeusUrlBuilder,
        JsonSerializerOptions jsonOptions)
    {
        _httpClientFactory = httpClientFactory;
        _context = context;
        _configuration = configuration;
        _authService = authService;
        _loggerService = loggerService;
        _urlBuilder = amadeusUrlBuilder;
        _jsonOptions = jsonOptions;
    }

    public async Task<AmadeusFlightOfferSearchResult> GetFlightOffersAsync(FlightOfferSearchRequestDTO searchRequestDTO)
    {
        await _loggerService.LogInfoAsync($"FlightOfferService received reqeust from API with ID: {searchRequestDTO.Id}");

#region CurrencyCode
        string _currencyCode = searchRequestDTO.CurrencyCode;
#endregion CurrencyCode

#region OriginDestinations
        // list of flights to be added to search to amadeus (and other providers)
        List<OriginDestination> _originDestinationList = new List<OriginDestination>();
        
        // first flight
        OriginDestination _originDestination1 = new OriginDestination
        {
            Id = "1",
            OriginLocationCode = searchRequestDTO.OriginLocationCode,
            DestinationLocationCode = searchRequestDTO.DestinationLocationCode,
            DateTimeRange = new DepartureDateTimeRange { Date = searchRequestDTO.DepartureDate },
        };
        _originDestinationList.Add(_originDestination1);

        // does second flight exist?
        if (searchRequestDTO.DepartureDateReturn is not null)
        {
            OriginDestination _originDestination2 = new OriginDestination
            {
                Id = "2",
                OriginLocationCode = searchRequestDTO.DestinationLocationCode,
                DestinationLocationCode = searchRequestDTO.OriginLocationCode,
                DateTimeRange = new DepartureDateTimeRange { Date = searchRequestDTO.DepartureDateReturn }
            };

            _originDestinationList.Add(_originDestination2);
        }
#endregion OriginDestinations

#region Travelers
        // list of travellers
        List<Traveler> _travelersList = new List<Traveler>();
        
        // create travellers
        foreach (var i in Enumerable.Range(1, searchRequestDTO.Adults))
        {
            Traveler _traveler = new Traveler
            {
                Id = i.ToString(),
                TravelerType = "ADULT",
                FareOptions = [ "STANDARD" ]
            };

            _travelersList.Add(_traveler);
        }
#endregion Travelers

#region Sources
        // nothing goes here, the code is already implemented in the class, do not create it again
#endregion Sources

#region SearchCriteria
        // create SearchCriteria
        SearchCriteria _searchCriteria = new SearchCriteria();

#region SearchCriteria.MaxFlightOffers
        // add the max flight offers value (if not null) and set the value else ignore, it will default to 10 results
        // nothing needs to be added, it gets added in this region
        if (searchRequestDTO.MaxFlightOffers == 0 || searchRequestDTO.MaxFlightOffers == 250)
        {
            _searchCriteria.MaxFlightOffers = 250;
        }
        if (searchRequestDTO.MaxFlightOffers > 0 || searchRequestDTO.MaxFlightOffers < 250)
        {
            _searchCriteria.MaxFlightOffers = searchRequestDTO.MaxFlightOffers;
        }
#endregion SearchCriteria.MaxFlightOffers
        
#region SearchCriteria.Filters (aka FlightFilters)
        // create the default FlightFilters object (it's got null initialization, so it's OK)
        FlightFilters _filters = new FlightFilters();
        // flight filters is made up of List<CabinRestriction>? and CarrierRestriction?, so
        // those will be created next, starting with CabinRestrictions

#region SearchCriteria.Filters.CabinRestrictions
        // create the .CabinRestrictions object (it will be appended to .Filters at the end of this region)
        List<CabinRestriction> _cabinRestrictions = new List<CabinRestriction>();

        // when adding to .CabinRestrictions, we need to know if there's a return
        // trip or oneway, so check if the return date exists, then create the
        // required item as an object, add it to a temp list, and add that list
        // to the searchCriteria object too
        if (searchRequestDTO.DepartureDateReturn is not null)
        {
            CabinRestriction _cabinRestrictionAllFlights = new CabinRestriction
            {
                Cabin = searchRequestDTO.CabinClass,
                Coverage = searchRequestDTO.CabinClassCoverage,
                OriginDestinationIds = [ "1", "2" ]
            };

            _cabinRestrictions.Add(_cabinRestrictionAllFlights);
        }
        else
        {
            CabinRestriction _cabinRestrictionAllFlights = new CabinRestriction
            {
                Cabin = searchRequestDTO.CabinClass,
                Coverage = searchRequestDTO.CabinClassCoverage,
                OriginDestinationIds = [ "1" ]
            };

            _cabinRestrictions.Add(_cabinRestrictionAllFlights);
        }

        _filters.CabinRestrictions = _cabinRestrictions;
#endregion SearchCriteria.Filters.CabinRestrictions

#region SearchCriteria.Filters.CarrierRestrictions
        // create the .CarrrierRestriction object (this gets added at the end of the region)
        CarrierRestriction _carrierRestriction = new CarrierRestriction();

        // do not edit the .BlacklistedInEUAllowed value (bool), it's set in the class

        // carrier excluded codes excludes, but carrier included restricts, so included is counted first then skip
        // else use excluded second
        if (searchRequestDTO.IncludedCarrierCodes is not null)
        {
            _carrierRestriction.IncludedCarrierCodes = SplitCommaSeparatedString(searchRequestDTO.IncludedCarrierCodes);
        }
        else if (searchRequestDTO.ExcludedCarrierCodes is not null)
        {
            _carrierRestriction.ExcludedCarrierCodes = SplitCommaSeparatedString(searchRequestDTO.ExcludedCarrierCodes);
        }

        _filters.CarrierRestrictions = _carrierRestriction;
#endregion SearchCriteria.FlightFilters.CarrierRestrictions
        
        _searchCriteria.Filters = _filters;
#endregion SearchCriteria.Filters (aka FlightFilters)

#endregion SearchCriteria

        // create the flight offer search (Amadeus)
        AmadeusFlightOfferSearch flightOfferSearch = new()
        {
            CurrencyCode = _currencyCode,
            OriginDestinations = _originDestinationList,
            Travelers = _travelersList ,
            SearchCriteria = _searchCriteria
        };

        //-->START DEBUG
        // this whole section is just debug code
        string debugJsonX = $@"/Users/danijeljw/Developer/dotnet-dev/Ava.API/searchRequestDTO_debugJsonX_{searchRequestDTO.Id}.json";
        await _loggerService.LogInfoAsync($"Saving results of {searchRequestDTO.Id} to {debugJsonX}");
        var xJson = JsonSerializer.Serialize(flightOfferSearch, _jsonOptions);
        await File.WriteAllTextAsync(debugJsonX, xJson);
        //-->END DEBUG

        // get the token information
        var token = await _authService.GetTokenInformationAsync();
        if (string.IsNullOrEmpty(token))
        {
            await _loggerService.LogErrorAsync("Unable to retrieve valid OAuth token.");
            throw new Exception("Unable to retrieve valid OAuth token.");
        }

        // create instance of HTTP client (from the factory)
        var httpClient = _httpClientFactory.CreateClient();
        httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        // get URL from appsettings to use
        string _flightOfferUrl = _configuration["Amadeus:Url:FlightOffer"]
            ?? throw new ArgumentNullException("Amadeus:Url:FlightOffer is missing in configuration.");

        var response = await httpClient.PostAsJsonAsync(_flightOfferUrl, flightOfferSearch, _jsonOptions);

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<AmadeusFlightOfferSearchResult>();
            return result ?? throw new InvalidOperationException("Deserialization returned null.");
        }

        else
        {
            Console.WriteLine($"Error: {response.StatusCode}");
            string errorBody = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Error Response: {errorBody}");
            throw new InvalidOperationException($"Error '{response.StatusCode}' Response: {errorBody}");
        }
    }

    private static readonly HashSet<string> AllowedTravelClasses = new()
    {
        "ECONOMY",
        "PREMIUM_ECONOMY",
        "BUSINESS",
        "FIRST"
    };

    private static List<string> SplitCommaSeparatedString(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return new List<string>();
        }

        return input.Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(s => s.Trim())
            .ToList();
    }
}
