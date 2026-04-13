namespace MellonBank.Infrastructure.Exceptions
{
    public abstract class InfrastructureException : Exception
    {
        public InfrastructureException(string message) : base(message)
        {
        }
    }
    public sealed class ExternalServiceException : InfrastructureException
    {
        public ExternalServiceException(string message) : base(message)
        {
        }
    }
}
