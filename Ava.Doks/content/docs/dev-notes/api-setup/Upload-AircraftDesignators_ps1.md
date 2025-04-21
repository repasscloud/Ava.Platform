---
title: "Upload-AircraftDesignators.ps1"
description: "This article explains how to use PowerShell to download a Wikipedia-sourced CSV and POST its contents into a local API, including model info, ICAO/IATA codes, and Wikipedia links."
summary: ""
date: 2023-09-07T16:13:18+02:00
lastmod: 2023-09-07T16:13:18+02:00
draft: false
weight: 910
toc: true
seo:
  title: "" # custom title (optional)
  description: "" # custom description (recommended)
  canonical: "" # custom canonical URL (optional)
  robots: "" # custom robot tags (optional)
---

This script is used to **ingest aircraft type designator data** from a public CSV file into a local API database. The CSV file is auto-generated weekly by a GitHub Action that scrapes [Wikipedia’s aircraft type designators](https://en.wikipedia.org/wiki/List_of_aircraft_type_designators) and saves a clean version to the repository.

The PowerShell script performs the following tasks:

- Downloads the CSV file from the `main` branch of the GitHub repo.
- Parses each line as a CSV row into a PowerShell object.
- For each record:
  - Creates a JSON payload following the API contract.
  - Excludes empty fields (`icao_code`, `iata_type_code`, `wikipedia_link`) to avoid sending nulls where they’re not required.
  - Sends the object to the API via a `POST` request to `http://localhost:5165/api/v1/wikipedia/aircrafttypedesignators`.
- Adds a small delay between each POST (`100ms`) to avoid overwhelming the local API.

This is useful for bootstrapping or syncing the Wikipedia aircraft data into your own application database, particularly when onboarding new environments or refreshing data as part of a nightly job.

```pwsh
# Define the CSV URL and target API endpoint
$csvUrl = "https://raw.githubusercontent.com/repasscloud/AircraftTypeDesignatorScraper/refs/heads/main/aircraft_type_designators.csv"
$apiEndpoint = "http://localhost:5165/api/v1/wikipedia/aircrafttypedesignators"

# Download CSV content
Write-Host "📥 Downloading CSV..."
$csvContent = Invoke-WebRequest -Uri $csvUrl -UseBasicParsing
if (-not $csvContent.Content) {
    Write-Error "Failed to download CSV."
    exit 1
}

# Convert CSV string to objects
Write-Host "🔍 Parsing CSV..."
$csv = $csvContent.Content | ConvertFrom-Csv

# Loop through each row and send as POST (throttled)
foreach ($row in $csv) {
    $payload = @{
        id = 0
        model = $row.'Model'
    }

    # Optional fields: include only if not blank
    if ($row.'ICAO Code' -and $row.'ICAO Code'.Trim() -ne "") {
        $payload["icao_code"] = $row.'ICAO Code'
    }
    if ($row.'IATA Type Code' -and $row.'IATA Type Code'.Trim() -ne "") {
        $payload["iata_type_code"] = $row.'IATA Type Code'
    }
    if ($row.'Wikipedia Link' -and $row.'Wikipedia Link'.Trim() -ne "") {
        $payload["wikipedia_link"] = $row.'Wikipedia Link'
    }

    # Convert to JSON
    $json = $payload | ConvertTo-Json -Depth 2 -Compress

    # Send the POST request
    try {
        $response = Invoke-RestMethod -Uri $apiEndpoint -Method Post -Body $json -ContentType "application/json"
        if ($null -ne $response) {
            Write-Host "✅ Posted: $($payload.model) — API Response: $response"
        }
    }
    catch {
        Write-Warning "❌ Failed to post: $($payload.model) — $_"
    }

    # Throttle to avoid hammering the API
    Start-Sleep -Milliseconds 100
}
```
