using JourneyMate.Domain.Common;
using JourneyMate.Domain.Events.UserEvents;

namespace JourneyMate.Domain.Entities;

public class User : BaseAuditableEntity
{
	public string Email { get; private set; }
	public string PasswordHash { get; private set; }
	public string UserName { get; private set; }
	public string? RefreshToken { get; private set; }
	public DateTime? RefreshTokenExpiryTime { get; private set; }
	public List<Role> Roles { get; private set; } = new();
	
	private User() { }
	public User(string email, string passwordHash, string userName)
	{
		Email = email;
		PasswordHash = passwordHash;
		UserName = userName;

		AddDomainEvent(new UserCreatedEvent(this));
	}

	public void ChangePassword(string passwordHash)
	{
		PasswordHash = passwordHash;

		AddDomainEvent(new UserPasswordUpdatedEvent(this));
	}

	public void SetRefreshToken(string token)
	{
		RefreshToken = token;
	}

	public void SetRefreshTokenExpiryTime(DateTime tokenExpireTime)
	{
		RefreshTokenExpiryTime = tokenExpireTime;
	}

	public bool IsTokenValid(string token, DateTime currentDate)
	{
		if (RefreshToken == token && RefreshTokenExpiryTime >= currentDate) return true;
		return false;
	}

	public void RemoveRefreshToken()
	{
		RefreshToken = null;
		RefreshTokenExpiryTime = null;
	}

	public void AddRole(Role role)
	{
		if (Roles.Contains(role)) return;
		Roles.Add(role);
	}

	public void RemoveRole(Role role)
	{
		if (!Roles.Contains(role)) return;
		Roles.Remove(role);
	}
}