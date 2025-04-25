namespace Ava.Shared.Models.DTOs;
public class FlightSearchResultDTO
{
    public required string Id { get; set; }
    public int NumberOfBookableSeats { get; set; }
    public int NumberOfSegments { get; set; }  // how many segments are flown?
    public TimeSpan Duration { get; set; }
    public string? IATACarrierCode { get; set; } // get this from data[x].validatingAirlineCodes
    public DateTime DepartingTime { get; set; }  // get this from data[x].itineraries[0].segments[x].departure.at
    public DateTime ArrivalTime { get; set; }  // time span against DepartingTime + Duration (above)
    public List<SegmentInfo>? Segments { get; set; } // data[x].itineraries[0].segments[x]
    public required string CurrencyCode { get; set; } // data[x].price.currency
    public required decimal TotalCost { get; set; }  // data[x].price.grandTotal
    public bool IncludedCheckedBags { get; set; } // data[x]

    public int TravelHours { get; set; }
    public int TravelMins { get; set; }

    // data[x].price.additionalServices ( list of kvp =>  amount:x, type:x)
}

public class SegmentInfo
{
    public required string SegmentId { get; set; } // data[x].inineraries[0].segments[x].id
    public required string DepartureIATACode { get; set; }
    public required string DepartureTerminal { get; set; }
    public required string DepartureAt { get; set; }
    public required string ArrivalIATACode { get; set;}
    public required string ArrivalTerminal { get; set;}
    public required string ArrivalAt { get; set;}
    public required string FlightNumber { get; set; }  // made from data[x].itineraries[0].segments[x]{.carrierCode + .number}
    public required string Aircraft { get; set; } // made from getting data[x].itineraries[0].segments[x].aircraft.code then do a lookup in dictionaries.aircraft.[lookup]
    public required string OperatingCarrier { get; set; } // this is the data[x].itineraries[0].segments[x].operating.carrierCode display the logo of AND check-in


    // use the SegmentId to go find this:
    // data[x].travelerPricings.fareDetailsBySegment => segmentId == segmentId
    // then do these:
    public required string Cabin { get; set; }
    public required string BrandedFareLabel { get; set; }

    // public bool? IncludedCheckedBags { get; set; }
    // quantity int
    // weight int
    // weightUnit string


    public bool? PrePaidBaggage { get; set; } // data[x].travelerPricings.fareDetailsBySegment[x].amenities => any => .isChargeable == true && any => .amenityType == BAGGAGE
    public bool? PrePaidBaggageCharged { get; set; }

    public bool? ComplimentaryBeverages { get; set; } // data[x].travelerPricings.fareDetailsBySegment[x].amenities => any => .description = "COMPLIMENTARY BEVERAGES && any => .isChargeable == false (then this is set to true)
    public bool? ComplimentaryBeveragesCharged { get; set; }

    // description MEAL OR SNACK
    public bool? MealOrSnack { get; set; }
    public bool? MealOrSnackCharged { get; set; }

    // domestic name change fee (is travel services)
    public bool? DomesticNameChange { get; set;}
    public bool? DomesticNameChangeCharged { get; set; }

    // standard seating
    public bool? StandardSeating { get; set; }
    public bool? StandardSeatingCharged { get; set; }

    // status credit accrual
    public bool? StatusCreditAccural { get; set; }
    public bool? StatusCreditAccuralCharged { get; set; }

    // points accrual
    public bool? PointsAccrual { get; set; }
    public bool? PointsAccrualCharged { get; set; }

    // dedicated check-in
    public bool? DedicatedCheckIn { get; set; }
    public bool? DedicatedCheckInCharged { get; set; }

    // priority boarding
    public bool? PriorityBoarding { get; set; }
    public bool? PriorityBoardingCharged { get; set; }

    public int? CheckedBagsWeight { get; set; }
    public string? CheckedBagsWeightUnit { get; set; }

    public bool? UsbPower { get; set; }
    public bool? UsbPowerCharged { get; set; }

    public bool? PriorityBaggage { get; set; }
    public bool? PriorityBaggageCharged { get; set; }

    public bool? PriorityImmigration { get; set; }
    public bool? PriorityImmigrationCharged  { get; set; }

    public bool? NoShow { get; set; }
    public bool? NoShowCharged { get; set; }

    public bool? BookingChange { get; set; }
    public bool? BookingChangeCharged { get; set; }
}
