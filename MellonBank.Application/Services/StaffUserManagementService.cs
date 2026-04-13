using FluentValidation;
using MellonBank.Application.DTOs.Requests;
using MellonBank.Application.DTOs.Responses;
using MellonBank.Application.Exceptions;
using MellonBank.Application.Interfaces.Services;
using MellonBank.Domain.Enums;

namespace MellonBank.Application.Services
{
    public class StaffUserManagementService : IStaffUserManagementService
    {
        private readonly IValidator<CreateUserRequestDto> _createValidator;
        private readonly IValidator<UpdateUserRequestDto> _updateValidator;
        private readonly IIdentityService _identityService;
        private readonly IRoleService _roleManagerService;
        private readonly ICurrentUserService _currentUserService;

        public StaffUserManagementService(
            IValidator<CreateUserRequestDto> createValidator, 
            IValidator<UpdateUserRequestDto> updateValidator, 
            IIdentityService identityService,
            IRoleService roleManagerService,
            ICurrentUserService currentUserService
        )
        {
            _identityService = identityService;
            _roleManagerService = roleManagerService;
            _currentUserService = currentUserService;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        public async Task<UserResponseDto> GetCustomerByAfmAsync(string afm, CancellationToken ct = default)
        {

            if(_currentUserService.UserId is null)
                throw new AppForbiddenException("User must be authenticated to access customer information.");

            if (!_currentUserService.IsInRole(RoleType.Staff.ToString()))
                throw new AppForbiddenException("Only staff users can create customers.");

            if (string.IsNullOrWhiteSpace(afm))
                throw new AppValidationException("AFM must be provided.");

            var user = await _identityService.GetByAfmAsync(afm, ct);
            if (user is null)
                throw new AppNotFoundException($"Customer with AFM {afm} not found.");

            if(! await _roleManagerService.IsInRoleAsync(user.Id, RoleType.Customer))
                throw new AppNotFoundException($"Customer with AFM {afm} not found.");


            return new UserResponseDto(
                Id: user.Id,
                FirstName: user.FirstName,
                LastName: user.LastName,
                Address: user.Address,
                Afm: user.Afm,
                PhoneNumber: user.PhoneNumber,
                Email: user.Email,
                UserName: user.UserName
            );
        }

        public async Task<IReadOnlyList<UserResponseDto?>> GetAllCustomersAsync(CancellationToken ct = default)
        {
            if (_currentUserService.UserId is null)
                throw new AppForbiddenException("User must be authenticated to access customer information.");

            if (!_currentUserService.IsInRole(RoleType.Staff.ToString()))
                throw new AppForbiddenException("Only staff users can view customers.");

            var users = await _identityService.GetUsersInRoleAsync(ct);

            return users.Any() ? users.Select(user => new UserResponseDto(
                Id: user.Id,
                FirstName: user.FirstName,
                LastName: user.LastName,
                Address: user.Address,
                Afm: user.Afm,
                PhoneNumber: user.PhoneNumber,
                Email: user.Email,
                UserName: user.UserName
            )).ToList() : new List<UserResponseDto>();
        }

        public async Task<string> CreateCustomerAsync(CreateUserRequestDto request, CancellationToken ct = default)
        {
            if (_currentUserService.UserId is null)
                throw new AppForbiddenException("User must be authenticated to access customer information.");

            if (!_currentUserService.IsInRole(RoleType.Staff.ToString()))
                throw new AppForbiddenException("Only staff users can create customers.");

            var result = await _createValidator.ValidateAsync(request, ct);
            if (!result.IsValid)
                throw new AppValidationException(result.ToDictionary());

            if(request.Role != RoleType.Customer)
                throw new AppValidationException("Invalid role specified for customer creation.");

            bool roleExists = await _roleManagerService.RoleExistsAsync(request.Role, ct);
            if (!roleExists)
                throw new AppValidationException($"Customer role does not exist.");

            var userId = await _identityService.CreateUserAsync(request, ct);

            if (userId is null)
                throw new Exception("An error occurred while creating the customer.");
            
            await _roleManagerService.AddToRoleAsync(userId, RoleType.Customer, ct);

            return userId;
        }

        public async Task<string> CreateStaffAsync(CreateUserRequestDto request, CancellationToken ct = default)
        {
            if (_currentUserService.UserId is null)
                throw new AppForbiddenException("User must be authenticated to access customer information.");

            if (!_currentUserService.IsInRole(RoleType.Staff.ToString()))
                throw new AppForbiddenException("Only staff users can create staff members.");

            var result = await _createValidator.ValidateAsync(request, ct);
            if (!result.IsValid)
                throw new AppValidationException(result.ToDictionary());

            if (request.Role != RoleType.Staff)
                throw new AppValidationException("Invalid role specified for staff creation.");

            var userId = await _identityService.CreateUserAsync(request, ct);

            if (userId is null)
                throw new Exception("An error occurred while creating the staff.");

            await _roleManagerService.AddToRoleAsync(userId, RoleType.Staff, ct);
            return userId;
        }

        public async Task UpdateCustomerAsync(string afm, UpdateUserRequestDto request, CancellationToken ct = default)
        {
            if (_currentUserService.UserId is null)
                throw new AppForbiddenException("User must be authenticated to access customer information.");

            if (!_currentUserService.IsInRole(RoleType.Staff.ToString()))
                throw new AppForbiddenException("Only staff users can update customers.");

            var user = await _identityService.GetByAfmAsync(afm, ct);
            if (user is null)
                throw new AppNotFoundException($"Customer with AFM {afm} not found.");

            if (!await _roleManagerService.IsInRoleAsync(user.Id, RoleType.Customer))
                throw new AppNotFoundException($"Customer with AFM {afm} not found.");

            var result = await _updateValidator.ValidateAsync(request, ct);
            if (!result.IsValid)
                throw new AppValidationException(result.ToDictionary());

            await _identityService.UpdateUserAsync(afm, request, ct);
        }

        public async Task DeleteCustomerAsync(string afm, CancellationToken ct = default)
        {
            if (_currentUserService.UserId is null)
                throw new AppForbiddenException("User must be authenticated to access customer information.");

            if (!_currentUserService.IsInRole(RoleType.Staff.ToString()))
                throw new AppForbiddenException("Only staff users can delete customers.");

            var user = await _identityService.GetByAfmAsync(afm, ct);
            if (user is null)
                throw new AppNotFoundException($"Customer with AFM {afm} not found.");

            if (!await _roleManagerService.IsInRoleAsync(user.Id, RoleType.Customer))
                throw new AppNotFoundException($"Customer with AFM {afm} not found.");

            await _identityService.DeleteUserAsync(afm, ct);
        }

    }
}
