using JourneyMate.Application.Common.Exceptions;
using JourneyMate.Application.Common.Interfaces;
using JourneyMate.Domain.Entities;
using JourneyMate.Domain.Repositories;
using JourneyMate.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace JourneyMate.Infrastructure.Security;
internal sealed class AuthorizationService : IAuthorizationService
{
	private readonly ApplicationDbContext _dbContext;
	private readonly IUserRepository _userRepository;

	public AuthorizationService(ApplicationDbContext dbContext, IUserRepository userRepository)
    {
		_dbContext = dbContext;
		_userRepository = userRepository;
	}

	public async Task<bool> AuthenticateUserAsync(Guid userId)
	{
		return await _dbContext.Users.AnyAsync(x => x.Id == userId);;
	}

	public async Task<bool> AuthorizeUserAsync(Guid userId, string roleName)
	{
		var user = await _dbContext.Users.Include(x => x.Roles).SingleOrDefaultAsync(x => x.Id == userId) ?? throw new UnauthorizedAccessException();
		return user.Roles.Select(x => x.Name).Contains(roleName);
	}

	public async Task AddUserToRoleAsync(Guid userId, string roleName)
	{
		var role = await _dbContext.Roles.SingleOrDefaultAsync(x => x.Name == roleName);
		if (role != null)
		{
			var user = await _userRepository.GetByIdAsync(userId);
			user.AddRole(role);
			await _userRepository.UpdateAsync(user);
		}
		else
		{
			throw new NotFoundException("Role not found");
		}
	}

	public async Task RemoveUserFromRoleAsync(Guid userId, string roleName)
	{
		var role = await _dbContext.Roles.SingleOrDefaultAsync(x => x.Name == roleName);
		if (role != null)
		{
			var user = await _userRepository.GetByIdAsync(userId);
			user.RemoveRole(role);
			await _userRepository.UpdateAsync(user);
		}
		else
		{
			throw new NotFoundException("Role not found");
		}
	}
}
