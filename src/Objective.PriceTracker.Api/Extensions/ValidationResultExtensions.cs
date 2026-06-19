using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Objective.PriceTracker.Api.Extensions;

public static class ValidationResultExtensions
{
    extension(ValidationResult result)
    {
        public void AddToModelState(ModelStateDictionary modelState)
        {
            foreach (var error in result.Errors)
            {
                modelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
        }
    }
}
