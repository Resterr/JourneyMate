namespace JourneyMate.Application.Common.Interfaces;

public interface IAuthorizationService
{
	Task<bool> AuthorizeAsync(Guid userId, string roleName);
	Task AddUserToRoleAsync(Guid userId, string roleName);
	Task RemoveUserFromRoleAsync(Guid userId, string roleName);
}