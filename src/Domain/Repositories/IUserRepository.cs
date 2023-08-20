using JourneyMate.Domain.Entities;

namespace JourneyMate.Domain.Repositories;
public interface IUserRepository
{
	Task<User> GetByIdAsync(Guid id);
	Task<User> GetByEmailAsync(string email);
	Task<User> GetByUserNameAsync(string userName);
	Task AddAsync(User user);
	Task UpdateAsync(User user);
	Task DeleteAsync(User user);
	Task<bool> HasDataCurrentlyUsedAsync(string? email, string? userName);
}
