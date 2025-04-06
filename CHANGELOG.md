
# Change Log
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/)
and this project adheres to [Semantic Versioning](http://semver.org/).

## [Unreleased] - yyyy-mm-dd

Here we write upgrading notes for brands. It's a team effort to make them as
straightforward as possible.

### Added

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
  Create AmenityV1 Extension Class [dev 95d1928]
- [#47](https://github.com/repasscloud/Ava.Platform/issues/47)
  Create FlightRouteRenderer Extension [dev 2d59a51]


### Changed

- [#11](https://github.com/repasscloud/Ava.WebApp/issues/11)
  🐳 Update image description label in `Dockerfile.builder` and related Docker/Compose files
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

### Fixed

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

### Removed

- [#22](https://github.com/repasscloud/Ava.WebApp/issues/22)
  Remove Solution files

### Backlog

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
  🧾 Create new Transaction table and model
- [#42](https://github.com/repasscloud/Ava.Platform/issues/42)
  Update login on booking page
- [#48](https://github.com/repasscloud/Ava.Platform/issues/48)
  Decide best visual indicator for layover airport mismatch (Blazor WebUI)
- [#44](https://github.com/repasscloud/Ava.Platform/issues/44)
  🐛 Add ISO 8601 Duration Parser Utility
