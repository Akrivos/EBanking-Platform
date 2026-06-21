namespace EBanking.Domain.Exceptions
{
    public abstract class DomainException : Exception
    {
        protected DomainException(string message) : base(message)
        {
        }
    }

    public class BusinessRuleException : DomainException
    {
        public BusinessRuleException(string message) : base(message)
        {
        }
    }

    public sealed class InsufficientBalanceException : BusinessRuleException
    {
        public InsufficientBalanceException()
            : base("Insufficient balance.")
        {
        }

        public InsufficientBalanceException(string message)
            : base(message)
        {
        }
    }

    public sealed class InvalidAccountOperationException : BusinessRuleException
    {
        public InvalidAccountOperationException(string message) : base(message)
        {
        }

    }
}
