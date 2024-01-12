using System.Transactions;
using AutoMapper;
using GameStore.Data.Entities.Identity;
using GameStore.Data.Extensions;
using GameStore.Data.Interfaces;
using GameStore.Services.Interfaces;
using GameStore.Shared.Constants;
using GameStore.Shared.DTOs.Role;
using GameStore.Shared.DTOs.User;
using GameStore.Shared.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Services;

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public UserService(
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager,
        IMapper mapper,
        IUnitOfWork unitOfWork)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<IList<UserBriefDto>> GetAllUsersAsync()
    {
        var users = await _userManager.Users.ToListAsync();
        return _mapper.Map<IList<UserBriefDto>>(users);
    }

    public async Task<UserBriefDto> GetUserByIdAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        ThrowIfUserWithIdNotFound(user, userId);
        return _mapper.Map<UserBriefDto>(user);
    }

    public async Task<IList<RoleBriefDto>> GetRolesByUserAsync(string userId)
    {
        var user = await _userManager.Users
            .Where(u => u.Id.Equals(userId))
            .Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
            .SingleOrDefaultAsync();

        ThrowIfUserWithIdNotFound(user, userId);
        var roles = user.UserRoles.Select(ur => ur.Role);
        return _mapper.Map<IList<RoleBriefDto>>(roles);
    }

    public async Task<UserBriefDto> RegisterUserAsync(UserRegistrationDto dto)
    {
        var user = new ApplicationUser(dto.User.Name);
        var roles = await GetRolesNamesByIdsAsync(dto.Roles);

        var transactionOptions = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted };
        using var scope = new TransactionScope(TransactionScopeOption.Required, transactionOptions, TransactionScopeAsyncFlowOption.Enabled);

        await _userManager.CreateAsync(user, dto.Password).ThrowIfFailedAsync();
        await _userManager.AddToRolesAsync(user, roles).ThrowIfFailedAsync();

        if (roles.Contains(Roles.Publisher))
        {
            await ConnectPublisherWithUserAccountAsync(user);
        }

        scope.Complete();

        return _mapper.Map<UserBriefDto>(user);
    }

    public async Task UpdateUserAsync(UserUpdateDto dto)
    {
        var user = await _userManager.Users
            .Where(u => u.Id.Equals(dto.User.Id))
            .Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
            .SingleOrDefaultAsync();

        ThrowIfUserWithIdNotFound(user, dto.User.Id);
        var userToUpdate = _mapper.Map(dto, user!);

        var transactionOptions = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted };
        using var scope = new TransactionScope(TransactionScopeOption.Required, transactionOptions, TransactionScopeAsyncFlowOption.Enabled);

        await _userManager.UpdateAsync(userToUpdate).ThrowIfFailedAsync();
        await AssignRolesToUserAsync(userToUpdate, dto.Roles);
        if (NeedToChangePassword(userToUpdate, dto.Password))
        {
            string? token = await _userManager.GeneratePasswordResetTokenAsync(userToUpdate);
            await _userManager.ResetPasswordAsync(userToUpdate, token, dto.Password).ThrowIfFailedAsync();
        }

        scope.Complete();
    }

    public async Task DeleteUserAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        ThrowIfUserWithIdNotFound(user, userId);
        await _userManager.DeleteAsync(user);
    }

    private static void ThrowIfUserWithIdNotFound(ApplicationUser? user, string id)
    {
        if (user is null)
        {
            throw new EntityNotFoundException(id);
        }
    }

    private Task<List<string>> GetRolesNamesByIdsAsync(IEnumerable<string> rolesIds)
    {
        return _roleManager.Roles
            .Where(r => rolesIds.Contains(r.Id))
            .Select(r => r.Name)
            .ToListAsync();
    }

    private async Task AssignRolesToUserAsync(ApplicationUser user, IEnumerable<string> rolesIds)
    {
        var currentRoles = user.UserRoles.Select(ur => ur.Role.Name);
        await _userManager.RemoveFromRolesAsync(user, currentRoles);
        var newRoles = await GetRolesNamesByIdsAsync(rolesIds);
        await _userManager.AddToRolesAsync(user, newRoles);
    }

    private bool NeedToChangePassword(ApplicationUser user, string newPassword)
    {
        if (user.PasswordHash is null)
        {
            return true;
        }

        var verificationResult = _userManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, newPassword);
        return verificationResult != PasswordVerificationResult.Success;
    }

    private async Task ConnectPublisherWithUserAccountAsync(ApplicationUser user)
    {
        try
        {
            var publisher = await _unitOfWork.Publishers.GetOneAsync(p => p.CompanyName == user.UserName, noTracking: false);
            if (publisher.AccountId is not null)
            {
                throw new IdentityException("The publisher is already connected to another user account.");
            }

            publisher.AccountId = user.Id;
            await _unitOfWork.SaveAsync();
        }
        catch (EntityNotFoundException)
        {
            throw new IdentityException($"When selecting a {Roles.Publisher} role, you must specify the exact name of the publisher.");
        }
    }
}