using FluentValidation;
using JourneyMate.Application.Common.Exceptions;
using JourneyMate.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JourneyMate.Application.Features.AddressFeature.Commands;

public record RemoveAddress(Guid Id) : IRequest<Unit>;

internal sealed class RemoveAddressHandler : IRequestHandler<RemoveAddress, Unit>
{
	private readonly IApplicationDbContext _dbContext;

	public RemoveAddressHandler(IApplicationDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task<Unit> Handle(RemoveAddress request, CancellationToken cancellationToken)
	{
		var address = await _dbContext.Addresses.SingleOrDefaultAsync(x => x.Id == request.Id) ?? throw new AddressNotFoundException(request.Id);

		_dbContext.Addresses.Remove(address);
		await _dbContext.SaveChangesAsync();

		return Unit.Value;
	}
}

public class RemoveAddressValidator : AbstractValidator<RemoveAddress>
{
	public RemoveAddressValidator()
	{
		RuleFor(x => x.Id)
			.NotEmpty();
	}
}