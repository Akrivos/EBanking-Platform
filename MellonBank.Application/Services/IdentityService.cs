using MellonBank.Application.Interfaces.Services;

namespace MellonBank.Application.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly IIdentityService _identityService;

        public IdentityService(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        //public 
    }
}
