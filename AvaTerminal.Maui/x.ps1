$pages = @(
    "FlightBookingPage",
    "TravelPolicyPage",
    "ClientPage",
    "UserDirectoryPage",
    "PnrPage",
    "TicketingPage",
    "ReconciliationPage",
    "SupportPage",
    "SalesPage",
    "InventoryPage",
    "MapPage",
    "TransactionPage",
    "NotificationsPage",
    "EnvironmentPage",
    "DebugPage",
    "ActivityLogPage",
    "AuthPage",
    "ConfigPage",
    "PatchNotesPage"
)

foreach ($page in $pages) {
    $pretty = [System.Text.RegularExpressions.Regex]::Replace(
        $page,
        '(?<!^)([A-Z])',
        { param($m) " $($m.Groups[1].Value)" }  # <- THIS is the real fix
    )
    [Console]::WriteLine($pretty)
}
