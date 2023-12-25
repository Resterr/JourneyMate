using AutoMapper;
using FluentValidation;
using JourneyMate.Application.Common.Exceptions;
using JourneyMate.Application.Common.Interfaces;
using JourneyMate.Application.Common.Mappings;
using JourneyMate.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JourneyMate.Application.Features.PlaceFeature.Queries;

public record GetAllReportsPaginated(int PageNumber, int PageSize) : IRequest<PaginatedList<ReportDto>>;

internal sealed class GetAllReportsHandler : IRequestHandler<GetAllReportsPaginated, PaginatedList<ReportDto>>
{
	private readonly IApplicationDbContext _dbContext;
	private readonly ICurrentUserService _currentUserService;
	private readonly IMapper _mapper;

	public GetAllReportsHandler(IApplicationDbContext dbContext, ICurrentUserService currentUserService, IMapper mapper)
	{
		_dbContext = dbContext;
		_currentUserService = currentUserService;
		_mapper = mapper;
	}

	public async Task<PaginatedList<ReportDto>> Handle(GetAllReportsPaginated request, CancellationToken cancellationToken)
	{
		var userId = _currentUserService.UserId ?? throw new UnauthorizedAccessException();
		var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId) ?? throw new UserNotFoundException(userId);
		var reports = await _dbContext.Reports.Where(x => x.UserId == user.Id).PaginatedListAsync(request.PageNumber, request.PageSize);
		var reportsDto = _mapper.Map<List<ReportDto>>(reports.Items);
		var result = new PaginatedList<ReportDto>(reportsDto, reports.TotalCount, request.PageNumber, request.PageSize);
		
		return result;
	}
}

public class GetAllReportsPaginatedValidator : AbstractValidator<GetAllReportsPaginated>
{
	public GetAllReportsPaginatedValidator()
	{
		RuleFor(x => x.PageNumber)
			.NotEmpty();
		RuleFor(x => x.PageSize)
			.NotEmpty();
	}
}