# Notes

## appsettings.json

### Client Id

Client ID is set in each installation for each client, they will use this to send requests to the main API to determine who's who in the zoo. It only comes from the webapp.

```json
"ClientSettings": {
    "ClientId": "abc123"
}
```


## Error Pages

### 1xx – System Errors:

Issues related to server startup, initialization, and configuration.

### 2xx – Authentication Errors:

Problems during login, such as invalid credentials or expired tokens.

### 3xx – User Account Errors:

Errors directly related to user account data (e.g. missing email, duplicate accounts).

### 4xx – Authorization Errors:

Errors when a user lacks the proper privileges or roles.

### 5xx – Data Validation Errors:

Input or validation issues, like incorrect formats or missing required fields.

### 6xx – Business Logic Errors:

Application-specific errors stemming from business rules or eligibility checks.

### 7xx – Integration/External Errors:

Failures with external systems or integration points (e.g. database or API connectivity).

### 8xx – Miscellaneous Errors:

Unhandled or unexpected errors that don’t fall into the other categories.

| Error Code | Category | Description | Suggested Action |
|:-----------|:---------|:------------|:-----------------|
| 101 | System (1xx) | Server initialization error | Check server startup logs and configuration. |
| 102 | System (1xx) | Configuration missing or invalid | Validate configuration files and environment settings. |
| 201 | Authentication (2xx) | Invalid credentials | Ask user to re-enter correct credentials. |
| 202 | Authentication (2xx) | Authentication token expired or invalid | Prompt user to re-authenticate or refresh token. |
| 301 | User Account (3xx) | User account not found | Verify user exists; check provided user ID or email. |
| 302 | User Account (3xx) | Missing email address | Ensure the email field is provided during registration. |
| 303 | User Account (3xx) | `[DEPRECIATED` Missing email address or account ID | Verify user data exists; check provided ID and email. |
| 304 | User Account (3xx) | Inactive account | Prompt user to activate their account via confirmation. |
| 305 | User Account (3xx) | Duplicate account detected | Inform user that the account already exists. |
| 306 | User Account (3xx) | Email already registered | Suggest password recovery or use a different email. |
| 307 | User Account (3xx) | Missing username | Require a valid username during registration. |
| 308 | User Account (3xx) | Login required | Redirect the user to the login page. |
| 401 | Authorization (4xx) | Insufficient privileges | Inform user they lack permission; advise contacting admin. |
| 402 | Authorization (4xx) | Role mismatch/access denied | Advise user to check their assigned roles/permissions. |
| 501 | Data Validation (5xx) | Invalid email format | Ask user to provide a properly formatted email address. |
| 502 | Data Validation (5xx) | Password does not meet complexity requirements | Provide password guidelines and request re-entry. |
| 503 | Data Validation (5xx) | Required fields are missing | Prompt user to complete all required fields. |
| 504 | Data Validation (5xx) | Required parameter missing in request | Ensure all required parametesr are included in the URL or form submission. |
| 601 | Business Logic (6xx) | Operation not permitted by business rules | Review business rules; adjust input or inform the user. |
| 602 | Business Logic (6xx) | User not eligible for the requested operation | Provide feedback on eligibility criteria. |
| 701 | Integration (7xx) | Database connection failed | Check database connectivity and configuration. |
| 702 | Integration (7xx) | External service timeout | Retry the operation or check the external service status. |
| 703 | Integration (7xx) | External integration/configuration error | Verify integration settings and external configurations. |
| 801 | Miscellaneous (8xx) | An unexpected error occurred | Log error details and instruct user to contact support. |
| 802 | Miscellaneous (8xx) | Service unavailable | Inform user to try again later or contact support. |