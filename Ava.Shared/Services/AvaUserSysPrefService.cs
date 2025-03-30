namespace Ava.Shared.Services;

public class AvaUserSysPrefService : IAvaUserSysPrefService
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;        
    private readonly ILoggerService _logger;

    public AvaUserSysPrefService(ApplicationDbContext context, UserManager<ApplicationUser> userManager, ILoggerService logger)
    {
        _context = context;
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<bool> CreateUserPrefRecordAsync(string aspNetUserId)
    {
        // TRACE: Entering the method.
        await _logger.LogTraceAsync($"Entering CreateUserPrefRecordAsync with aspNetUserId: {aspNetUserId}");
        
        try
        {
            // DEBUG: Attempting to locate user by id.
            await _logger.LogDebugAsync($"Looking for user with id: {aspNetUserId}");
            var user = await _userManager.FindByIdAsync(aspNetUserId);

            if (user is not null)
            {
                // INFO: User found; proceeding to create default system preference.
                await _logger.LogInfoAsync($"User found with id: {aspNetUserId}. Creating default system preference record.");
                
                AvaUserSysPreference avaUserSysPref = new AvaUserSysPreference
                {
                    Id = 0,
                    AspNetUsersId = aspNetUserId,
                    IsActive = true,
                    Email = user.NormalizedEmail?.ToLowerInvariant() ?? string.Empty,
                    FirstName = string.Empty,
                    LastName = string.Empty,
                    DefaultFlightSeating = "ECONOMY",
                    MaxFlightSeating = "ECOMOMY",
                    CabinClassCoverage = "MOST_SEGMENTS",
                    DefaultCurrencyCode = "AUD",
                    MaxFlightPrice = 0,
                    MaxResults = 20,
                    EnableSaturdayFlightBookings = false,
                    EnableSundayFlightBookings = false
                };

                // DEBUG: Adding system preference record to context.
                await _logger.LogDebugAsync($"Adding AvaUserSysPreference record for user id: {aspNetUserId} to context.");
                await _context.AvaUserSysPreferences.AddAsync(avaUserSysPref);

                // DEBUG: Saving changes to the database.
                await _logger.LogDebugAsync($"Saving changes to the database for user id: {aspNetUserId}.");
                int affectedRows = await _context.SaveChangesAsync();

                if (affectedRows > 0)
                {
                    // INFO: Successfully created the system preference record.
                    await _logger.LogInfoAsync($"Successfully created system preference record for user id: {aspNetUserId}.");
                    return true;
                }
                
                // WARNING: SaveChanges did not affect any rows.
                await _logger.LogWarningAsync($"No rows affected while creating system preference record for user id: {aspNetUserId}.");
                return false;
            }
            else
            {
                // WARNING: User not found.
                await _logger.LogWarningAsync($"User not found with id: {aspNetUserId}.");
            }
        }
        catch (Exception ex)
        {
            // CRITICAL: An exception occurred during the operation.
            await _logger.LogCriticalAsync($"An exception occurred in CreateUserPrefRecordAsync for user id: {aspNetUserId}. Exception: {ex}");
            return false;
        }
        finally
        {
            // TRACE: Exiting the method.
            await _logger.LogTraceAsync($"Exiting CreateUserPrefRecordAsync for user id: {aspNetUserId}.");
        }

        // ERROR: If execution reaches here, something unexpected happened.
        await _logger.LogErrorAsync($"Unexpected path reached in CreateUserPrefRecordAsync for user id: {aspNetUserId}.");
        return false;
    }

    public async Task<AvaUserSysPreference> GetUserPrefRecordAsync(string aspNetUserId)
    {
        // TRACE: Entering the method.
        await _logger.LogTraceAsync($"Entering GetUserPrefRecordAsync for user id: {aspNetUserId}.");

        try
        {
            // DEBUG: Attempting to retrieve the user preference record.
            await _logger.LogDebugAsync($"Attempting to retrieve user preference record for user id: {aspNetUserId}.");
            var userPref = await _context.AvaUserSysPreferences
                .FirstOrDefaultAsync(x => x.AspNetUsersId == aspNetUserId);

            if (userPref != null)
            {
                // INFO: User preference record found.
                await _logger.LogInfoAsync($"User preference record found for user id: {aspNetUserId}.");
                return userPref;
            }
            else
            {
                // WARNING: Record not found, attempting to create one.
                await _logger.LogWarningAsync($"User preference record not found for user id: {aspNetUserId}. Attempting to create one.");
                bool created = await CreateUserPrefRecordAsync(aspNetUserId);

                if (created)
                {
                    // DEBUG: Record created successfully, now retrieving the record.
                    await _logger.LogDebugAsync($"User preference record successfully created for user id: {aspNetUserId}. Retrieving record.");
                    userPref = await _context.AvaUserSysPreferences
                        .FirstOrDefaultAsync(x => x.AspNetUsersId == aspNetUserId);

                    if (userPref != null)
                    {
                        // INFO: Successfully retrieved the newly created record.
                        await _logger.LogInfoAsync($"User preference record retrieved successfully for user id: {aspNetUserId}.");
                        return userPref;
                    }
                    else
                    {
                        // ERROR: Record creation succeeded but retrieval failed.
                        await _logger.LogErrorAsync($"User preference record was created but could not be retrieved for user id: {aspNetUserId}.");
                        throw new Exception("User preference record retrieval failed after creation.");
                    }
                }
                else
                {
                    // ERROR: Record creation failed.
                    await _logger.LogErrorAsync($"Failed to create user preference record for user id: {aspNetUserId}.");
                    throw new Exception("User preference record creation failed.");
                }
            }
        }
        catch (Exception ex)
        {
            // CRITICAL: Exception occurred during the process.
            await _logger.LogCriticalAsync($"Exception in GetUserPrefRecordAsync for user id: {aspNetUserId}. Exception: {ex}");
            throw new Exception($"Exception: {ex}");
        }
        finally
        {
            // TRACE: Exiting the method.
            await _logger.LogTraceAsync($"Exiting GetUserPrefRecordAsync for user id: {aspNetUserId}.");
        }
    }


    public async Task<bool> UpdateUserPrefRecordAsync(AvaUserSysPreference userSysPreference)
    {
        // TRACE: Entering the method.
        await _logger.LogTraceAsync($"Entering UpdateUserPrefRecordAsync for user id: {userSysPreference.AspNetUsersId} and record id: {userSysPreference.Id}.");

        try
        {
            // DEBUG: Attempting to retrieve the existing record matching both AspNetUserId and Id.
            await _logger.LogDebugAsync($"Retrieving record for user id: {userSysPreference.AspNetUsersId} with record id: {userSysPreference.Id}.");
            var existingRecord = await _context.AvaUserSysPreferences
                .FirstOrDefaultAsync(r => r.Id == userSysPreference.Id && r.AspNetUsersId == userSysPreference.AspNetUsersId);

            if (existingRecord != null)
            {
                // DEBUG: Found the record; updating fields.
                await _logger.LogDebugAsync($"Record found for user id: {userSysPreference.AspNetUsersId} and record id: {userSysPreference.Id}. Updating record fields.");

                // Update the record's fields (excluding Id and AspNetUsersId)
                existingRecord.IsActive = userSysPreference.IsActive;
                existingRecord.Email = userSysPreference.Email.ToLowerInvariant();
                existingRecord.FirstName = userSysPreference.FirstName;
                existingRecord.LastName = userSysPreference.LastName;
                existingRecord.DefaultFlightSeating = userSysPreference.DefaultFlightSeating;
                existingRecord.MaxFlightSeating = userSysPreference.MaxFlightSeating;
                existingRecord.CabinClassCoverage = userSysPreference.CabinClassCoverage;
                existingRecord.DefaultCurrencyCode = userSysPreference.DefaultCurrencyCode;
                existingRecord.MaxFlightPrice = userSysPreference.MaxFlightPrice;
                existingRecord.MaxResults = userSysPreference.MaxResults;
                existingRecord.EnableSaturdayFlightBookings = userSysPreference.EnableSaturdayFlightBookings;
                existingRecord.EnableSundayFlightBookings = userSysPreference.EnableSundayFlightBookings;

                // DEBUG: Saving changes to the database.
                await _logger.LogDebugAsync($"Saving updated record for user id: {userSysPreference.AspNetUsersId} and record id: {userSysPreference.Id}.");
                int affectedRows = await _context.SaveChangesAsync();

                if (affectedRows > 0)
                {
                    // INFO: Record updated successfully.
                    await _logger.LogInfoAsync($"Successfully updated user preference record for user id: {userSysPreference.AspNetUsersId} and record id: {userSysPreference.Id}.");
                    return true;
                }
                else
                {
                    // WARNING: No rows were affected during the update.
                    await _logger.LogWarningAsync($"No rows affected while updating record for user id: {userSysPreference.AspNetUsersId} and record id: {userSysPreference.Id}.");
                    return false;
                }
            }
            else
            {
                // WARNING: No matching record was found.
                await _logger.LogWarningAsync($"User preference record not found for user id: {userSysPreference.AspNetUsersId} and record id: {userSysPreference.Id}.");
                return false;
            }
        }
        catch (Exception ex)
        {
            // CRITICAL: An exception occurred during the update.
            await _logger.LogCriticalAsync($"Exception in UpdateUserPrefRecordAsync for user id: {userSysPreference.AspNetUsersId} and record id: {userSysPreference.Id}. Exception: {ex}");
            return false;
        }
        finally
        {
            // TRACE: Exiting the method.
            await _logger.LogTraceAsync($"Exiting UpdateUserPrefRecordAsync for user id: {userSysPreference.AspNetUsersId} and record id: {userSysPreference.Id}.");
        }
    }
}
