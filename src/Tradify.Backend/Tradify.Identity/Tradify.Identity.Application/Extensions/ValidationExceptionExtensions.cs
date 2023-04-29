using System.Runtime.Serialization;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Tradify.Identity.Application.Responses.Errors;

public static class ValidationExceptionExtensions
{
    public static ValidationProblemDetails ToProblemDetails(
        this ValidationException ex)
    {
	    var error = new ValidationProblemDetails()
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            Status = 400,
        };

        foreach (var validationFailure in ex.Errors)
        {
            if (error.Errors.ContainsKey(validationFailure.PropertyName))
            {
                error.Errors[validationFailure.PropertyName]
                    = error.Errors[validationFailure.PropertyName]
                        .Concat(new[] { validationFailure.ErrorMessage }).ToArray();
                continue;
            }
            
            error.Errors.Add(new KeyValuePair<string, string[]>(
                validationFailure.PropertyName,
                new []{validationFailure.ErrorMessage}));
        }

        return error;
    }
}