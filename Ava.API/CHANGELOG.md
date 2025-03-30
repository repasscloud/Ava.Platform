
# Change Log
All notable changes to this project will be documented in this file.
 
The format is based on [Keep a Changelog](http://keepachangelog.com/)
and this project adheres to [Semantic Versioning](http://semver.org/).
 
## [v0.0.3] - 2025-03-23
 
**Configuration Update:** The project configuration was switched to use Docker Compose, streamlining the deployment process.

**NuGet Package Upgrades:** Critical packages were updated—Microsoft.AspNetCore.OpenApi, Microsoft.EntityFrameworkCore (and its Design package), and Npgsql.EntityFrameworkCore.PostgreSQL—ensuring the project benefits from the latest improvements and bug fixes.

**Shared Library Integration:** The project was merged with the Ava.Shared library, centralizing class integration and reducing code duplication.

**Directory Cleanup:** The obsolete _help directory was removed as its contents have been migrated to another project.

**Build Issues Resolved:** Build errors were fixed by merging with Ava.Shared, updating Controllers, and removing obsolete classes.
 
### Changed

- [#21](https://github.com/repasscloud/Ava.API/issues/21)
  Swap to docker compose config.

- [Update Nuget packages #22](https://github.com/repasscloud/Ava.API/issues/22)
  Microsoft.AspNetCore.OpenApi `9.0.1` -> `9.0.3`  
  Microsoft.EntityFrameworkCore `9.0.1` -> `9.0.3`  
  Microsoft.EntityFrameworkCore.Design `9.0.1` -> `9.0.3`
  Npgsql.EntityFrameworkCore.PostgreSQL `9.0.3` -> `9.0.4`

- [Merge with Ava.Shared project #25](https://github.com/repasscloud/Ava.API/issues/25)
  Moved over to shared library for class integration using `Ava.Shared` basename.

- [Remove _help directory #24](https://github.com/repasscloud/Ava.API/issues/24)
  `_help` directory has been migrated to a different project, it is no longer required

### Fixed

- [Build errors #23](https://github.com/repasscloud/Ava.API/issues/23)
  Merged with `Ava.Shared` library and rectify Controllers and remove classes
