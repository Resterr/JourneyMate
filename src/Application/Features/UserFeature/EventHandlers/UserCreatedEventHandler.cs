using JourneyMate.Domain.Events.UserEvents;
using MediatR;
using Microsoft.Extensions.Logging;

namespace JourneyMate.Application.Features.UserFeature.EventHandlers;
public class UserCreatedEventHandler : INotificationHandler<UserCreatedEvent>
{
	private readonly ILogger<UserCreatedEventHandler> _logger;

	public UserCreatedEventHandler(ILogger<UserCreatedEventHandler> logger)
	{
		_logger = logger;
	}

	public Task Handle(UserCreatedEvent notification, CancellationToken cancellationToken)
	{
		_logger.LogInformation("JourneyMate Domain Event: {DomainEvent}", notification.GetType().Name);

		return Task.CompletedTask;
	}
}
