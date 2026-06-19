using FluentValidation;
using Objective.PriceTracker.Api.Models.Requests;

namespace Objective.PriceTracker.Api.Validators;

public class AddSubscriptionRequestValidator : AbstractValidator<AddSubscriptionRequest>
{
    public AddSubscriptionRequestValidator()
    {
        RuleFor(x => x.Mail).NotEmpty().EmailAddress();
        RuleFor(x => x.Url)
            .NotEmpty()
            .Matches("https://prinzip.su/flats/(?<complexName>[0-9a-z_-]+)/(?<flatId>[0-9]+)/");
    }
}
