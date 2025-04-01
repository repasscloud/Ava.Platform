namespace Ava.API.Controllers.Flights;

[Route("api/v1/flights")]
[ApiController]
public class FlightsController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IAmadeusAuthService _authService;  // for the amadeus search itself
    private readonly IAmadeusFlightSearchService _flightSearchService;
    private readonly ILoggerService _loggerService;
    private readonly JsonSerializerOptions _jsonOptions;
    
    public FlightsController(
        ApplicationDbContext context,
        IAmadeusAuthService authService,
        IAmadeusFlightSearchService flightSearchService,
        ILoggerService loggerService,
        JsonSerializerOptions jsonOptions)
    {
        _context = context;
        _authService = authService;
        _flightSearchService = flightSearchService;
        _loggerService = loggerService;
        _jsonOptions = jsonOptions;
    }
    
    // POST: api/v1/flights/search
    [HttpPost("search")]
    public async Task<ActionResult<AmadeusFlightOfferSearchResult>> SearchFlights(FlightOfferSearchRequestDTO criteria)
    {
        // add .CreatedAt value (this must be controlled by API, it will be ignored by everything else)
        criteria.CreatedAt = DateTime.UtcNow;

        // save it to the database
        await _loggerService.LogInfoAsync($"Received flight search record with ID '{criteria.Id}' created at '{criteria.CreatedAt}'");

        // save the search to history (to be used later?)
        _context.FlightOfferSearchRequestDTOs.Add(criteria);
        await _context.SaveChangesAsync();
        await _loggerService.LogDebugAsync($"Executing flight search record with ID: {criteria.Id}");

        // all logging happns in the _flightSearchService for the .GetFlightOffersAsync() activity
        var response = await _flightSearchService.GetFlightOffersAsync(criteria);

        if (response is not null)
        {
            await _loggerService.LogDebugAsync($"Sending API response for flight search record with ID: {criteria.Id}");
            return Ok(response); // returns HTTP 200 with JSON
        }

        return NotFound("The response from the Amadeus API was empty or invalid.");
    }
}
