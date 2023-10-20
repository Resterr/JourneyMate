using JourneyMate.Domain.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JourneyMate.Infrastructure.Common;
public static class MediatorExtensions
{
	public static async Task DispatchDomainEvents(this IMediator mediator, DbContext context)
	{
		var entities = context.ChangeTracker
			.Entries<BaseEntity>()
			.Where(e => e.Entity.DomainEvents.Any())
			.Select(e => e.Entity);

		var baseEntities = entities.ToList();
		var domainEvents = baseEntities
			.SelectMany(e => e.DomainEvents)
			.ToList();

		baseEntities.ForEach(e => e.ClearDomainEvents());

		foreach (var domainEvent in domainEvents)
			await mediator.Publish(domainEvent);
	}
}
