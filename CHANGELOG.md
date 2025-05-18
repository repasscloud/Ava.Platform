
# Change Log

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/)
and this project adheres to [Semantic Versioning](http://semver.org/).

## [0.2.0-alpha] - yyyy-MM-dd

### Added

### Changed

- [#69](https://github.com/repasscloud/Ava.WebApp/issues/69)
  Remove HTTPS redirection in webapi #69

### Fixed

### Removed

## [0.1.0-alpha] - 2025-04-10

Version `0.1.0-alpha` is not production ready. Still WIP.

### Added [0.1.0-alpha] - 2025-04-10

- [#10](https://github.com/repasscloud/Ava.WebApp/issues/10)
  📌 Create trigger to clean up old entries in `StorageEntries` table
- [#7](https://github.com/repasscloud/Ava.WebApp/issues/7)
  Add IsOneWay flag to FlightOfferSearchRequestDTO
- [#18](https://github.com/repasscloud/Ava.WebApp/issues/18)
  Create support error page for missing parameters
- [#45](https://github.com/repasscloud/Ava.Platform/issues/45)
  💱 Add static helper to convert currency codes to symbols #45
- [#43](https://github.com/repasscloud/Ava.Platform/issues/43)
  Create FlightResultV1 class for WebApp display
- [#46](https://github.com/repasscloud/Ava.Platform/issues/46)
  Create AmenityV1 Extension Class [95d1928]
- [#47](https://github.com/repasscloud/Ava.Platform/issues/47)
  Create FlightRouteRenderer Extension [2d59a51]
- [#53](https://github.com/repasscloud/Ava.WebApp/issues/53)
  Create debug page for FlightResults [f6d0439]

### Changed [0.1.0-alpha] - 2025-04-10

- [#11](https://github.com/repasscloud/Ava.WebApp/issues/11)
  🐳 Update image description label in `Dockerfile.builder` and related Docker/Compose files [c1fb6bb] [9a35200]
- [#6](https://github.com/repasscloud/Ava.WebApp/issues/6)
  Update TravelPolicy to include "DepartAfterTime" and "DepartBeforeTime"
- [#3](https://github.com/repasscloud/Ava.WebApp/issues/3)
  Move Search controller for WebApp to it's own category
- [#12](https://github.com/repasscloud/Ava.WebApp/issues/12)
  Update Migrations
- [#19](https://github.com/repasscloud/Ava.WebApp/issues/19)
  Refactor Travel Policy page
- [#20](https://github.com/repasscloud/Ava.WebApp/issues/20)
  Refactor Book Flight screen
- [#21](https://github.com/repasscloud/Ava.WebApp/issues/21)
  Refactor code
- [#23](https://github.com/repasscloud/Ava.WebApp/issues/23)
  TravelPolicyID updated from int to string
- [#31](https://github.com/repasscloud/Ava.WebApp/issues/31)
  Swap TravelPolicyID from int to string
- [#16](https://github.com/repasscloud/Ava.WebApp/issues/16)
  Update TravelPolicy
- [#15](https://github.com/repasscloud/Ava.WebApp/issues/15)
  Update AvaUserSysPreference Class
- [#25](https://github.com/repasscloud/Ava.WebApp/issues/25)
  Extend ApplicationUser Model to Include Custom Profile Fields
- [#14](https://github.com/repasscloud/Ava.WebApp/issues/14)
  Extend ApplicationUser Model to Include Custom Profile Fields
- [#27](https://github.com/repasscloud/Ava.WebApp/issues/27)
  Update ILoggerService to use ENUM for writing logs
- [#29](https://github.com/repasscloud/Ava.WebApp/issues/29)
  Swap to Docker Compose config
- [#49](https://github.com/repasscloud/Ava.WebApp/issues/49)
  Swap ClientID from 10 ALPHANUMERIC to 10 HEXADECIMAL characters [0833165]
- [#59](https://github.com/repasscloud/Ava.Platform/issues/59)
  Optimize Local Storage Usage in Blazor by Offloading Large Data to Backend [251682d] [dd7c5dc]

### Fixed [0.1.0-alpha] - 2025-04-10

- [#8](https://github.com/repasscloud/Ava.WebApp/issues/8)
  WebApp missing key details in the flightOfferSearchRequest
- [#5](https://github.com/repasscloud/Ava.WebApp/issues/5)
  Add TravelPolicy to the FlightOfferSearchRequestDTO
- [#17](https://github.com/repasscloud/Ava.WebApp/issues/17)
  Update currency codes
- [#24](https://github.com/repasscloud/Ava.WebApp/issues/24)
  TravelPolicy not deserializing on /Book/Flight
- [#32](https://github.com/repasscloud/Ava.WebApp/issues/32)
  TPCityIATACodeService.cs uses wrong ILogger
- [#13](https://github.com/repasscloud/Ava.WebApp/issues/13)
  Remove SQLite support
- [#26](https://github.com/repasscloud/Ava.WebApp/issues/26)
  Update UserSysPreference
- [#28](https://github.com/repasscloud/Ava.WebApp/issues/28)
  Blazor circuit error
- [#50](https://github.com/repasscloud/Ava.WebApp/issues/50)
  Improve Travel Policy display for "None" allowed regions/continents/countries
- [#51](https://github.com/repasscloud/Ava.WebApp/issues/51)
  Remove icon from Book Flight page #51 [4067a22]
- [#52](https://github.com/repasscloud/Ava.WebApp/issues/52)
  FlightOffer Missing operating.carrierCode defaults to "__" [25030e6]
- [#54](https://github.com/repasscloud/Ava.WebApp/issues/54)
  Add Localization to Docker container runtime [da56fb4] [ffeff10]
- [#55](https://github.com/repasscloud/Ava.Platform/issues/55)
  `/api/v1/flight/search` not storing TravelPolicyID [8835423] [dd7c5dc]

### Removed [0.1.0-alpha] - 2025-04-10

- [#22](https://github.com/repasscloud/Ava.WebApp/issues/22)
  Remove Solution files

## Backlog

- [#9](https://github.com/repasscloud/Ava.WebApp/issues/9)
  📦 Persist Data Protection Keys in Docker
- [#4](https://github.com/repasscloud/Ava.WebApp/issues/4)
  Extend AvaClient with billing default
- [#34](https://github.com/repasscloud/Ava.WebApp/issues/34)
  Update model for SearchFlights
- [#33](https://github.com/repasscloud/Ava.WebApp/issues/33)
  QANTAS Amenities
- [#30](https://github.com/repasscloud/Ava.WebApp/issues/30)
  Create Flight Results Screen
- [#35](https://github.com/repasscloud/Ava.WebApp/issues/35)
  💳 Extend TravelPolicy to include DefaultPaymentMethods #35
- [#36](https://github.com/repasscloud/Ava.Platform/issues/36)
  💰 Extend AvaClient model to include markup percentage on quote prices
- [#37](https://github.com/repasscloud/Ava.Platform/issues/37)
  🧾 Add flat service fee field to AvaClient
- [#38](https://github.com/repasscloud/Ava.Platform/issues/38)
  💳 Add default payment method flags to AvaClient
- [#39](https://github.com/repasscloud/Ava.Platform/issues/39)
  📅 Add annual and monthly commitment fields to AvaClient
- [#40](https://github.com/repasscloud/Ava.Platform/issues/40)
  💸 Track cumulative spend (annual/monthly) on AvaClient
- [#41](https://github.com/repasscloud/Ava.Platform/issues/41)
  🧾 Create new Transaction table and model [34d96dc]
- [#42](https://github.com/repasscloud/Ava.Platform/issues/42)
  Update login on booking page
- [#48](https://github.com/repasscloud/Ava.Platform/issues/48)
  Decide best visual indicator for layover airport mismatch (Blazor WebUI)
- [#44](https://github.com/repasscloud/Ava.Platform/issues/44)
  🐛 Add ISO 8601 Duration Parser Utility
- [#56](https://github.com/repasscloud/Ava.Platform/issues/56)
  Design new routes for handling flights search results [4a020bf] [dd7c5dc]
- [#57](https://github.com/repasscloud/Ava.Platform/issues/57)
  Refactor Flight Search Results to Use Unique ID-Based Records [0209c63] [8dd80c9] [dd7c5dc]
- [#58](https://github.com/repasscloud/Ava.Platform/issues/58)
  Define and Handle Record Types for Search Results [f5e9d79] [dd7c5dc]
- [#60](https://github.com/repasscloud/Ava.Platform/issues/60)
  Add 30-Day Expiry to All Search and DTO Records in SQL
- [#62](https://github.com/repasscloud/Ava.Platform/issues/62)
  Enable Flight Result Info Button
