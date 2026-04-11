using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace MellonBank.Application
{
    public static class DependencyInjection
    {
        public static void AddApplication(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
