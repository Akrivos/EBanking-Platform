using FluentValidation.Results;

namespace MellonBank.Application.Common.Validation
{
    public static class ValidationExtensions
    {
        public static IReadOnlyDictionary<string, string[]> ToDictionary(this ValidationResult result)
        {
            return result.Errors
                .GroupBy(x => x.PropertyName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(x => x.ErrorMessage).ToArray()
                );
        }
    }
}
