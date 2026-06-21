using FluentValidation;
using EBanking.Application.Common.Models;
using EBanking.Application.DTOs.Requests;
using EBanking.Application.Exceptions;
using EBanking.Application.Interfaces.Services;

namespace EBanking.Application.Services
{
    public sealed class ProfileService : IProfileService
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IValidator<ChangePasswordRequestDto> _changePasswordValidator;
        private readonly IIdentityService _identityService;

        public ProfileService(
            ICurrentUserService currentUserService,
            IValidator<ChangePasswordRequestDto> changePasswordValidator,
            IIdentityService identityService)
        {
            _currentUserService = currentUserService;
            _changePasswordValidator = changePasswordValidator;
            _identityService = identityService;
        }

        public async Task<Result> ChangePasswordAsync(ChangePasswordRequestDto request, CancellationToken ct = default)
        {
            var currentUser = _currentUserService.UserId;
            if (currentUser is null)
                throw new AppForbiddenException("User must be authenticated to change password.");

            var validationResult = await _changePasswordValidator.ValidateAsync(request, ct);
            if (!validationResult.IsValid)
                return Result.Failure("Please correct the password fields.");

            return await _identityService.ChangePasswordAsync(
                currentUser,
                request.CurrentPassword,
                request.NewPassword,
                ct);
        }
    }
}