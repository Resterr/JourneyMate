using FluentValidation;
using JourneyMate.Application.Common.Exceptions;
using JourneyMate.Application.Common.Interfaces;
using JourneyMate.Domain.Entities;
using JourneyMate.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JourneyMate.Application.Features.PlaceFeature.Commands;

public record GenerateReport(Guid AddressId, List<string> Types) : IRequest<Guid>;

internal sealed class GenerateReportHandler : IRequestHandler<GenerateReport, Guid>
{
	private readonly IApplicationDbContext _dbContext;
	private readonly IApplicationMongoClient _mongoClient;
	private readonly ICurrentUserService _currentUserService;

	public GenerateReportHandler(IApplicationDbContext dbContext, IApplicationMongoClient mongoClient, ICurrentUserService currentUserService)
	{
		_dbContext = dbContext;
		_mongoClient = mongoClient;
		_currentUserService = currentUserService;
	}

	public async Task<Guid> Handle(GenerateReport request, CancellationToken cancellationToken)
	{
		var userId = _currentUserService.UserId ?? throw new UnauthorizedAccessException();
		var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId) ?? throw new UserNotFoundException(userId);

		var address = await _dbContext.Addresses.FirstOrDefaultAsync(x => x.Id == request.AddressId) ?? throw new AddressNotFound(request.AddressId);
		var types = await _dbContext.PlaceTypes.Where(x => request.Types.Contains(x.Name)).ToListAsync(cancellationToken);

		var placeAddresses = await _dbContext.PlaceAddress.Include(x => x.Place)
			.ThenInclude(x => x.Types)
			.Where(x => x.AddressId == address.Id)
			.ToListAsync(cancellationToken);

		var places = placeAddresses.Select(x => x.Place)
			.ToList();
		
		places = places.Where(x => x.CheckType(types)).ToList();
		places = places.Where(x => x.UserRatingsTotal > 100 && x.Rating >= 4.0).ToList();

		var reportId = Guid.NewGuid();
		var placesId = places.Select(x => x.Id).ToList();
		var newReport = new Report(reportId, user.Id, request.AddressId, placesId, request.Types);
		
		await _mongoClient.Reports.InsertOneAsync(newReport);

		return reportId;
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