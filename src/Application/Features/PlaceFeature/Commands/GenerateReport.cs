using FluentValidation;
using JourneyMate.Application.Common.Exceptions;
using JourneyMate.Application.Common.Interfaces;
using JourneyMate.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JourneyMate.Application.Features.PlaceFeature.Commands;

public record GenerateReport(Guid AddressId, List<string> Types) : IRequest<Guid>;

internal sealed class GenerateReportHandler : IRequestHandler<GenerateReport, Guid>
{
	private readonly IApplicationDbContext _dbContext;
	private readonly ICurrentUserService _currentUserService;

	public GenerateReportHandler(IApplicationDbContext dbContext, ICurrentUserService currentUserService)
	{
		_dbContext = dbContext;
		_currentUserService = currentUserService;
	}

	public async Task<Guid> Handle(GenerateReport request, CancellationToken cancellationToken)
	{
		var userId = _currentUserService.UserId ?? throw new UnauthorizedAccessException();
		var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId) ?? throw new UserNotFoundException(userId);

		var address = await _dbContext.Addresses.FirstOrDefaultAsync(x => x.Id == request.AddressId) ?? throw new AddressNotFoundException(request.AddressId);
		var types = await _dbContext.PlaceTypes.Where(x => request.Types.Contains(x.Name)).ToListAsync(cancellationToken);

		var placeAddresses = await _dbContext.PlaceAddress.Include(x => x.Place)
			.ThenInclude(x => x.Types)
			.Where(x => x.AddressId == address.Id && x.Place.UserRatingsTotal > 10 && x.Place.Rating > 3.0)
			.ToListAsync(cancellationToken);

		var places = placeAddresses.Select(x => x.Place)
			.OrderBy(x => x.Rating)
			.ToList();
		
		places = places.Where(x => x.CheckType(types)).ToList();
		
		var newReport = new Report(user, address, places, types);

		await _dbContext.Reports.AddAsync(newReport);
		await _dbContext.SaveChangesAsync();

		return newReport.Id;
	}
}

public class GenerateReportValidator : AbstractValidator<GenerateReport>
{
	public GenerateReportValidator()
	{
		RuleFor(x => x.AddressId)
			.NotEmpty();
		RuleFor(x => x.Types)
			.NotEmpty();
	}
}