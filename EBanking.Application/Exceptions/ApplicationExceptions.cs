namespace EBanking.Application.Exceptions
{
    public abstract class ApplicationException : Exception
    {
        public ApplicationException(string message) : base(message)
        {
        }
    }

    public sealed class AppNotFoundException : ApplicationException
    {
        public AppNotFoundException(string message) : base(message)
        {
        }
    }

    public sealed class AppForbiddenException : ApplicationException
    {
        public AppForbiddenException(string message) : base(message)
        {
        }
    }

    public sealed class AppConflictException : ApplicationException
    {
        public AppConflictException(string message) : base(message)
        {
        }
    }

    public sealed class AppUnauthorizedException : ApplicationException
    {
        public AppUnauthorizedException(string message) : base(message)
        {
        }
    }

    public sealed class AppValidationException : ApplicationException
    {
        public IDictionary<string, string[]> Errors { get; }

        public AppValidationException(string message)
            : base(message)
        {
            Errors = new Dictionary<string, string[]>
            {
                { "General", new[] { message } }
            };
        }
        public AppValidationException(IDictionary<string, string[]> errors)
            : base("One or more validation errors occurred.")
        {
            Errors = errors;
        }
    }

    public sealed class ExternalServiceException : ApplicationException
    {
        public ExternalServiceException(string message) : base(message)
        { 
        }
    }

    public sealed class AppInternalServerErrorException : ApplicationException
    {
        public AppInternalServerErrorException(string message) : base(message)
        {
        }
    }
}
