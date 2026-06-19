using Objective.PriceTracker.Api.Models.Prinzip;

namespace Objective.PriceTracker.Api.Services.Interfaces;

public interface IPrinzipApi
{
    Task<PrinzipApartment?> GetApartmentDataAsync(
        int apartmentId,
        CancellationToken cancellationToken
    );

    Task<PrinzipProject?> GetProjectDataAsync(int projectId, CancellationToken cancellationToken);
}
