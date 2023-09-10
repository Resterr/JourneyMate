using JourneyMate.Application.Common.Exceptions;
using JourneyMate.Application.Common.Interfaces;
using JourneyMate.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace JourneyMate.Infrastructure.Services;
internal sealed class AvailabilityService : IAvailabilityService
{
	private readonly ApplicationDbContext _dbContext;

	public AvailabilityService(ApplicationDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task<bool> CheckAddress(string placeId)
	{
		var query = _dbContext.Addresses;

		if (await query.AnyAsync(x => x.PlaceId == placeId))
		{
			throw new BadRequestException("PlaceId already taken.");
		}

		return true;
	}

	public async Task<bool> CheckUser(string? email, string? userName)
	{
		var query = _dbContext.Users;

		if (email != null)
		{
			if (await query.AnyAsync(x => x.Email == email))
			{
				throw new BadRequestException("Email already taken.");
			}
		}

		if (userName != null)
		{
			if (await query.AnyAsync(x => x.UserName == userName))
			{
				throw new BadRequestException("Username already taken.");
			}
		}

		return true;
	}
}
