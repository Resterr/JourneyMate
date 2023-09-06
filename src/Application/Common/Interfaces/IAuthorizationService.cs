namespace JourneyMate.Application.Common.Interfaces;
public interface IAuthorizationService
{
	Task<bool> AuthenticateUserAsync(Guid userId);
	Task<bool> AuthorizeUserAsync(Guid userId, string roleName);
	Task AddUserToRoleAsync(Guid userId, string roleName);
	Task RemoveUserFromRoleAsync(Guid userId, string roleName);
}
