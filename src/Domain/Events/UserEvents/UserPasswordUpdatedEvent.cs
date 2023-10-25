using JourneyMate.Domain.Common;
using JourneyMate.Domain.Entities;

namespace JourneyMate.Domain.Events.UserEvents;

public class UserPasswordUpdatedEvent : BaseEvent
{
	public User User { get; }

	public UserPasswordUpdatedEvent(User user)
	{
		User = user;
	}
}