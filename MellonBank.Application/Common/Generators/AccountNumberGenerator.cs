using MellonBank.Application.Interfaces.Services;

namespace MellonBank.Application.Common.Generators
{
    public class AccountNumberGenerator : IAccountNumberGenerator
    {
        public string Generate()
        {
            var randomPart = Random.Shared.NextInt64(1000000000, 9999999999);
            return $"MB{randomPart}";
        }
    }
}
