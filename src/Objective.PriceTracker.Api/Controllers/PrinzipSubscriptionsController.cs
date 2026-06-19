using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Objective.PriceTracker.Api.Extensions;
using Objective.PriceTracker.Api.Models;
using Objective.PriceTracker.Api.Models.Requests;
using Objective.PriceTracker.Api.Models.Responses;
using Objective.PriceTracker.Api.Services.Interfaces;

namespace Objective.PriceTracker.Api.Controllers;

[ApiController]
[Route("api/v1/subscriptions/prinzip")]
public class PrinzipSubscriptionsController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType<List<PrinzipApartmentPrice>>(200)]
    public async Task<IActionResult> GetCurrentSubscriptions(
        [FromQuery] string mail,
        [FromServices] IPrinzipSubscriptionHandler subscriptionHandler,
        CancellationToken cancellationToken
    )
    {
        return Ok(
            await subscriptionHandler.GetCurrentSubscribedApartments(mail, cancellationToken)
        );
    }

    [HttpPost("subscribe")]
    [ProducesResponseType<SubscriptionResult>(200)]
    public async Task<IActionResult> SubscribeToChangesAsync(
        [FromBody] AddSubscriptionRequest request,
        [FromServices] IValidator<AddSubscriptionRequest> requestValidator,
        [FromServices] IPrinzipSubscriptionHandler subscriptionHandler,
        CancellationToken cancellationToken
    )
    {
        var validationResult = await requestValidator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState);
            return BadRequest(ModelState);
        }

        var result = await subscriptionHandler.HandleSubscriptionAsync(
            request.Mail,
            request.Url,
            cancellationToken
        );

        if (result is Models.SubscriptionResult.Error)
        {
            return Conflict();
        }

        return Ok(result);
    }
}
