using FluentValidation;
using JourneyMate.Application.Common.Exceptions;
using JourneyMate.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JourneyMate.Application.Features.PlaceFeature.Commands;

public record RateReport(Guid ReportId, int Rate) : IRequest<Unit>;

internal sealed class RateReportHandler : IRequestHandler<RateReport, Unit>
{
	private readonly ICurrentUserService _currentUserService;
	private readonly IApplicationDbContext _dbContext;

	public RateReportHandler(IApplicationDbContext dbContext, ICurrentUserService currentUserService)
	{
		_dbContext = dbContext;
		_currentUserService = currentUserService;
	}

	public async Task<Unit> Handle(RateReport request, CancellationToken cancellationToken)
	{
		var userId = _currentUserService.UserId ?? throw new UnauthorizedAccessException();
		var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId) ?? throw new UserNotFoundException(userId);
		var report = await _dbContext.Reports.Where(x => x.UserId == user.Id).FirstOrDefaultAsync(x => x.Id == request.ReportId) ?? throw new ReportNotFoundException(request.ReportId);
		
		report.UpdateRating(request.Rate);
		_dbContext.Reports.Update(report);
		await _dbContext.SaveChangesAsync();
		
		return Unit.Value;
	}
}

public class RateReportValidator : AbstractValidator<RateReport>
{
	public RateReportValidator()
	{
		RuleFor(x => x.ReportId)
			.NotEmpty();
		RuleFor(x => x.Rate)
			.NotEmpty().GreaterThanOrEqualTo(1).LessThanOrEqualTo(5);
	}
}