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
		
		var types = await _dbContext.PlaceTypes.Where(x => request.Types.Contains(x.Name)).ToListAsync(cancellationToken);

		var places = await _dbContext.Places.Include(x => x.Address)
			.Include(x => x.Types)
			.Where(x => x.AddressId == request.AddressId)
			.ToListAsync(cancellationToken);

		places = places.Where(x => types.Any(y => types.Contains(y))).ToList();
		places = places.Where(x => x.UserRatingsTotal > 100 && x.Rating >= 4.0).ToList();

		var reportId = Guid.NewGuid();
		var placesId = places.Select(x => x.Id).ToList();
		var newReport = new Report(reportId, user.Id, placesId);
		
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