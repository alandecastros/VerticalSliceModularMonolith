using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Results;

namespace VerticalSliceModularMonolith.Infrastructure.FluentValidation;

public class AutoFluentValidationAutoValidationCustomResultFactory : IFluentValidationAutoValidationResultFactory
{
    public IActionResult CreateActionResult(ActionExecutingContext context, ValidationProblemDetails? validationProblemDetails)
    {
        return validationProblemDetails is not null
            ? new BadRequestObjectResult(new { error = validationProblemDetails.Errors.Values.First()[0], message = validationProblemDetails.Errors.Values.First()[0] })
            : new BadRequestObjectResult(new { error = "Tivemos um problema inesperado", message = "Tivemos um problema inesperado", description = "Por favor entre em contato com o suporte" });
    }
}