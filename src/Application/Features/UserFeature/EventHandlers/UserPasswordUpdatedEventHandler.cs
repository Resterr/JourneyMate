using JourneyMate.Domain.Events.UserEvents;
using MediatR;
using Microsoft.Extensions.Logging;

namespace JourneyMate.Application.Features.UserFeature.EventHandlers;

public class UserPasswordUpdatedEventHandler : INotificationHandler<UserPasswordUpdatedEvent>
{
	private readonly ILogger<UserPasswordUpdatedEventHandler> _logger;

	public UserPasswordUpdatedEventHandler(ILogger<UserPasswordUpdatedEventHandler> logger)
	{
		_logger = logger;
	}

	public Task Handle(UserPasswordUpdatedEvent notification, CancellationToken cancellationToken)
	{
		_logger.LogInformation("JourneyMate Domain Event: {DomainEvent}", notification.GetType()
			.Name);

		return Task.CompletedTask;
	}
}