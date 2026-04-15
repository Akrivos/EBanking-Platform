using MellonBank.Application.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace MellonBank.Web.ErrorHandling
{
    public static class ExceptionToProblemDetailsMapper
    {
        public static ProblemDetails Map(Exception exception)
        {
            return exception switch
            {
                AppValidationException ex => new ValidationProblemDetails(ex.Errors)
                {
                    Title = ex.Message,
                    Status = StatusCodes.Status400BadRequest
                },

                AppUnauthorizedException ex => Create(ex.Message, StatusCodes.Status401Unauthorized),
                AppForbiddenException ex => Create(ex.Message, StatusCodes.Status403Forbidden),
                AppNotFoundException ex => Create(ex.Message, StatusCodes.Status404NotFound),
                AppConflictException ex => Create(ex.Message, StatusCodes.Status409Conflict),
                ExternalServiceException ex => Create(ex.Message, StatusCodes.Status502BadGateway),

                _ => Create("An unexpected error occurred.", StatusCodes.Status500InternalServerError)
            };
        }

        private static ProblemDetails Create(string message, int statusCode)
        {
            return new ProblemDetails
            {
                Title = message,
                Status = statusCode
            };
        }
    }
}
