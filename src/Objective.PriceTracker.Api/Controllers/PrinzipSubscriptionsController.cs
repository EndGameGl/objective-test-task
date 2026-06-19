using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Objective.PriceTracker.Api.Extensions;
using Objective.PriceTracker.Api.Models.Requests;
using Objective.PriceTracker.Api.Services.Interfaces;

namespace Objective.PriceTracker.Api.Controllers;

[ApiController]
[Route("api/v1/subscriptions/prinzip")]
public class PrinzipSubscriptionsController : ControllerBase
{
    [HttpPost("subscribe")]
    public async Task<IActionResult> SubscribeToChangesAsync(
        [FromBody] AddSubscriptionRequest request,
        [FromServices] IValidator<AddSubscriptionRequest> requestValidator,
        [FromServices] IPrinzipSubscriptionHandler subscriptionHandler
    )
    {
        var validationResult = await requestValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState);
            return BadRequest(ModelState);
        }

        var result = await subscriptionHandler.HandleSubscriptionAsync(request.Mail, request.Url);

        if (result is Models.SubscriptionResult.Error)
        {
            return Conflict();
        }

        return Ok(result);
    }
}
