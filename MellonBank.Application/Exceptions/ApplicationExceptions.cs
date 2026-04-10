namespace MellonBank.Application.Exceptions
{
    public abstract class ApplicationException : Exception
    {
        public ApplicationException(string message) : base(message)
        {
        }
    }

    public sealed class NotFoundException : ApplicationException
    {
        public NotFoundException(string message) : base(message)
        {
        }
    }

    public sealed class ForbiddenException : ApplicationException
    {
        public ForbiddenException(string message) : base(message)
        {
        }
    }

    public sealed class ConflictException : ApplicationException
    {
        public ConflictException(string message) : base(message)
        {
        }
    }

    public sealed class ValidationException : ApplicationException
    {
        public IReadOnlyDictionary<string, string[]> Errors { get; }

        public ValidationException(string message) : base(message)
        {
            Errors = new Dictionary<string, string[]>();
        }

        public ValidationException(IReadOnlyDictionary<string, string[]> errors)
            : base("One or more validation errors occurred.")
        {
            Errors = errors;
        }
    }
}
