﻿using JourneyMate.Application.Common.Exceptions;
using JourneyMate.Domain.Entities;
using JourneyMate.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace JourneyMate.Infrastructure.Persistence.Repositories;

internal sealed class UserRepository : IUserRepository
{
	private readonly ApplicationDbContext _dbContext;

	public UserRepository(ApplicationDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task<List<User>> GetAllAsync()
	{
		var query = await _dbContext.Users.Include(x => x.Roles)
			.ToListAsync();
		var superAdmin = query.Where(user => user.Roles.Any(role => role.Name == "SuperAdmin"))
			.ToList();

		query.Remove(superAdmin[0]);

		return query;
	}

	public async Task<User> GetByIdAsync(Guid id)
	{
		var query = await _dbContext.Users.SingleOrDefaultAsync(x => x.Id == id) ?? throw new UserNotFoundException(id);

		return query;
	}

	public async Task<User> GetByEmailAsync(string email)
	{
		var query = await _dbContext.Users.SingleOrDefaultAsync(x => x.Email == email) ?? throw new UserNotFoundException(email, "email");

		return query;
	}

	public async Task<User> GetByUserNameAsync(string userName)
	{
		var query = await _dbContext.Users.SingleOrDefaultAsync(x => x.UserName == userName) ?? throw new UserNotFoundException(userName, "username");

		return query;
	}

	public async Task AddAsync(User user)
	{
		await _dbContext.Users.AddAsync(user);
		await _dbContext.SaveChangesAsync();
	}

	public async Task UpdateAsync(User user)
	{
		_dbContext.Users.Update(user);
		await _dbContext.SaveChangesAsync();
	}

	public async Task DeleteAsync(User user)
	{
		_dbContext.Users.Remove(user);
		await _dbContext.SaveChangesAsync();
	}
}