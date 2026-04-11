using FluentValidation;
using MellonBank.Application.DTOs.Requests;
using MellonBank.Application.Exceptions;
using MellonBank.Application.Interfaces.Services;
using MellonBank.Domain.Enums;

namespace MellonBank.Application.Services
{
    public class CustomerManagementService : ICustomerManagementService
    {
        private readonly IValidator<CreateUserRequestDto> _createValidator;
        private readonly IIdentityService _identityService;
        private readonly ICurrentUserService _currentUserService;

        public CustomerManagementService(IValidator<CreateUserRequestDto> createValidator, IIdentityService identityService, ICurrentUserService currentUserService)
        {
            _identityService = identityService;
            _currentUserService = currentUserService;
            _createValidator = createValidator;
        }

        public async Task<string> CreateCustomerAsync(CreateUserRequestDto request, CancellationToken ct = default)
        {
            if (!_currentUserService.IsInRole(RoleType.Staff.ToString()))
                throw new AppForbiddenException("Only staff users can create customers.");

            var result = await _createValidator.ValidateAsync(request, ct);
            if (!result.IsValid)
                throw new AppValidationException(result.ToDictionary());

            var userId = await _identityService.CreateUserAsync(request, ct);
            return userId;
        }

        public async Task<string> CreateStaffAsync(CreateUserRequestDto request, CancellationToken ct = default)
        {
            if (!_currentUserService.IsInRole(RoleType.Staff.ToString()))
                throw new AppForbiddenException("Only staff users can create staff members.");

            var result = await _createValidator.ValidateAsync(request, ct);
            if (!result.IsValid)
                throw new AppValidationException(result.ToDictionary());

            var userId = await _identityService.CreateUserAsync(request, ct);
            return userId;
        }

        public async Task DeleteCustomerAsync(string afm, CancellationToken ct = default)
        {
            if (!_currentUserService.IsInRole(RoleType.Staff.ToString()))
                throw new AppForbiddenException("Only staff users can delete customers.");

            var user = await _identityService.GetByAfmAsync(afm, ct);
            if (user is null)
                throw new AppNotFoundException($"Customer with AFM {afm} not found.");

            await _identityService.DeleteUserAsync(afm, ct);
        }
    }
}
