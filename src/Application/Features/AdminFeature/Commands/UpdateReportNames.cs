using JourneyMate.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JourneyMate.Application.Features.AdminFeature.Commands;

public record UpdateReportNames : IRequest<Unit>;

internal sealed class UpdateReportNamesHandler : IRequestHandler<UpdateReportNames, Unit>
{
	private readonly IApplicationDbContext _dbContext;

	public UpdateReportNamesHandler(IApplicationDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task<Unit> Handle(UpdateReportNames request, CancellationToken cancellationToken)
	{
		var groupedReports = _dbContext.Reports.Include(x => x.User).ThenInclude(x => x.Reports)
			.OrderBy(x => x.Created)
			.GroupBy(x => x.UserId)
			.ToList();
		
		foreach (var group in groupedReports)
		{
			var index = 1;
			foreach (var report in group)
			{
				report.ReportNumber = index;
				report.UpdateName();
				index++;
			}
		}
		
		var reports = groupedReports.SelectMany(group => group).ToList();
		
		_dbContext.Reports.UpdateRange(reports);
		await _dbContext.SaveChangesAsync(cancellationToken);

		return Unit.Value;
	}
}