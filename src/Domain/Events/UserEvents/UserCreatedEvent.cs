using JourneyMate.Domain.Common;
using JourneyMate.Domain.Entities;

namespace JourneyMate.Domain.Events.UserEvents;

public class UserCreatedEvent : BaseEvent
{
	public User User { get; }

	public UserCreatedEvent(User user)
	{
		User = user;
	}
}