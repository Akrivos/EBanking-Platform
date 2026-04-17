using FluentValidation;
using MellonBank.Application.Common.Models;
using MellonBank.Application.DTOs.Requests;
using MellonBank.Application.DTOs.Responses;
using MellonBank.Application.Exceptions;
using MellonBank.Application.Interfaces.Repositories;
using MellonBank.Application.Interfaces.Services;
using MellonBank.Domain.Enums;

namespace MellonBank.Application.Services
{
    public sealed class StaffUserManagementService : IStaffUserManagementService
    {
        private readonly IValidator<CreateUserRequestDto> _createValidator;
        private readonly IValidator<UpdateUserRequestDto> _updateValidator;
        private readonly IIdentityService _identityService;
        private readonly IRoleService _roleManagerService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IBankAccountRepository _bankAccountRepository;

        public StaffUserManagementService(
            IValidator<CreateUserRequestDto> createValidator,
            IValidator<UpdateUserRequestDto> updateValidator,
            IIdentityService identityService,
            IRoleService roleManagerService,
            ICurrentUserService currentUserService,
            IBankAccountRepository bankAccountRepository)
        {
            _identityService = identityService;
            _roleManagerService = roleManagerService;
            _currentUserService = currentUserService;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
           _bankAccountRepository = bankAccountRepository;
        }

        public async Task<UserResponseDto?> GetCustomerByAfmAsync(string afm, CancellationToken ct = default)
        {
            EnsureStaffAccess();

            if (string.IsNullOrWhiteSpace(afm))
                throw new AppValidationException("AFM must be provided.");

            var user = await _identityService.GetByAfmAsync(afm, ct);
            if (user is null)
                throw new AppNotFoundException($"Customer with AFM {afm} not found.");

            if (!await _roleManagerService.IsInRoleAsync(user.Id, RoleType.Customer))
                throw new AppNotFoundException($"Customer with AFM {afm} not found.");

            return new UserResponseDto(
                FirstName: user.FirstName,
                LastName: user.LastName,
                Afm: user.Afm,
                PhoneNumber: user.PhoneNumber,
                Email: user.Email,
                UserName: user.UserName,
                Address: user.Address
            );
        }

        public async Task<IEnumerable<UserResponseDto?>> GetAllCustomersAsync(CancellationToken ct = default)
        {
            EnsureStaffAccess();

            var users = await _identityService.GetUsersInRoleAsync(RoleType.Customer, ct);

            return users.Any()
                ? users.Select(user => new UserResponseDto(
                    FirstName: user!.FirstName,
                    LastName: user.LastName,
                    Afm: user.Afm,
                    PhoneNumber: user.PhoneNumber,
                    Email: user.Email,
                    UserName: user.UserName,
                    Address: user.Address
                )).ToList()
                : new List<UserResponseDto>();
        }

        public async Task<Result<string>> CreateCustomerAsync(CreateUserRequestDto request, CancellationToken ct = default)
        {
            EnsureStaffAccess();

            if (request.Role != RoleType.Customer)
                return Result<string>.Failure("Invalid role specified for customer creation.");

            var validationResult = await _createValidator.ValidateAsync(request, ct);

            if (!validationResult.IsValid)
                return Result<string>.Failure("Validation failed.");

            bool roleExists = await _roleManagerService.RoleExistsAsync(request.Role, ct);
            if (!roleExists)
                return Result<string>.Failure("Customer role does not exist.");

            var existingUser = await _identityService.GetByAfmAsync(request.Afm, ct);
            if (existingUser is not null)
                return Result<string>.Failure($"Customer with AFM {request.Afm} already exists.");

            var userId = await _identityService.CreateUserAsync(request, ct);
            if (userId is null)
                return Result<string>.Failure("An error occurred while creating the customer.");

            await _roleManagerService.AddToRoleAsync(userId, RoleType.Customer, ct);

            return Result<string>.Success(userId);
        }

        public async Task<Result<string>> CreateStaffAsync(CreateUserRequestDto request, CancellationToken ct = default)
        {
            EnsureStaffAccess();

            if (request.Role != RoleType.Staff)
                return Result<string>.Failure("Invalid role specified for staff creation.");

            var validationResult = await _createValidator.ValidateAsync(request, ct);
            if (!validationResult.IsValid)
                return Result<string>.Failure("Validation failed.");

            bool roleExists = await _roleManagerService.RoleExistsAsync(request.Role, ct);
            if (!roleExists)
                return Result<string>.Failure("Staff role does not exist.");

            var existingUser = await _identityService.GetByAfmAsync(request.Afm, ct);

            if (existingUser is not null)
                return Result<string>.Failure($"User with AFM {request.Afm} already exists.");

            var userId = await _identityService.CreateUserAsync(request, ct);
            if (userId is null)
                return Result<string>.Failure("An error occurred while creating the staff user.");

            await _roleManagerService.AddToRoleAsync(userId, RoleType.Staff, ct);

            return Result<string>.Success(userId);
        }

        public async Task<Result> UpdateCustomerAsync(string afm, UpdateUserRequestDto request, CancellationToken ct = default)
        {
            EnsureStaffAccess();

            var user = await _identityService.GetByAfmAsync(afm, ct);
            if (user is null)
                throw new AppNotFoundException($"Customer with AFM {afm} not found.");

            if (!await _roleManagerService.IsInRoleAsync(user.Id, RoleType.Customer))
                throw new AppNotFoundException($"Customer with AFM {afm} not found.");

            var validationResult = await _updateValidator.ValidateAsync(request, ct);
            if (!validationResult.IsValid)
                return Result.Failure("Validation failed.");

            await _identityService.UpdateUserAsync(afm, request, ct);

            return Result.Success();
        }

        public async Task<Result> DeleteCustomerAsync(string afm, CancellationToken ct = default)
        {
            EnsureStaffAccess();

            var user = await _identityService.GetByAfmAsync(afm, ct);
            if (user is null)
                throw new AppNotFoundException($"Customer with AFM {afm} not found.");

            if (!await _roleManagerService.IsInRoleAsync(user.Id, RoleType.Customer))
                throw new AppNotFoundException($"Customer with AFM {afm} not found.");

            var hasAccounts = await _bankAccountRepository.AnyAccountByUserIdAsync(user.Id, ct);

            if (hasAccounts)
                return Result.Failure("This customer cannot be deleted because they are associated with one or more bank accounts.");

            await _identityService.DeleteUserAsync(afm, ct);

            return Result.Success();
        }

        private void EnsureStaffAccess()
        {
            if (_currentUserService.UserId is null)
                throw new AppForbiddenException("User must be authenticated to access customer information.");

            if (!_currentUserService.IsInRole(RoleType.Staff.ToString()))
                throw new AppForbiddenException("Only staff users can perform this action.");
        }
    }
}