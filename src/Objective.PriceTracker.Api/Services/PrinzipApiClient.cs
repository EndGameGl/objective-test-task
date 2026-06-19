using Objective.PriceTracker.Api.Models.Prinzip;
using Objective.PriceTracker.Api.Services.Interfaces;

namespace Objective.PriceTracker.Api.Services;

public class PrinzipApiClient : IPrinzipApi
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<PrinzipApiClient> _logger;

    public PrinzipApiClient(HttpClient httpClient, ILogger<PrinzipApiClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<PrinzipApartment?> GetApartmentDataAsync(
        int apartmentId,
        CancellationToken cancellationToken
    )
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<PrinzipApartment>(
                $"/api/v1/public/apartments/{apartmentId}/",
                cancellationToken
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get data for apartment {ApartmentId}", apartmentId);
            return null;
        }
    }

    public async Task<PrinzipProject?> GetProjectDataAsync(
        int projectId,
        CancellationToken cancellationToken
    )
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<PrinzipProject>(
                $"/api/v1/public/projects/{projectId}/",
                cancellationToken
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get data for project {ProjectId}", projectId);
            return null;
        }
    }
}
