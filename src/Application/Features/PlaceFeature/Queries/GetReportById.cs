using AutoMapper;
using FluentValidation;
using JourneyMate.Application.Common.Exceptions;
using JourneyMate.Application.Common.Interfaces;
using JourneyMate.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JourneyMate.Application.Features.PlaceFeature.Queries;

public record GetReportById(Guid Id) : IRequest<ReportDto>;

internal sealed class GetReportByIdHandler : IRequestHandler<GetReportById, ReportDto>
{
	private readonly IApplicationDbContext _dbContext;
	private readonly ICurrentUserService _currentUserService;
	private readonly IMapper _mapper;

	public GetReportByIdHandler(IApplicationDbContext dbContext, ICurrentUserService currentUserService, IMapper mapper)
	{
		_dbContext = dbContext;
		_currentUserService = currentUserService;
		_mapper = mapper;
	}

	public async Task<ReportDto> Handle(GetReportById request, CancellationToken cancellationToken)
	{
		var userId = _currentUserService.UserId ?? throw new UnauthorizedAccessException();
		var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId) ?? throw new UserNotFoundException(userId);
		var report = await _dbContext.Reports.Where(x => x.UserId == user.Id).FirstOrDefaultAsync(x => x.Id == request.Id) ?? throw new ReportNotFound(request.Id);
		var result = _mapper.Map<ReportDto>(report);
		
		return result;
	}
}

public class GetReportByIdValidator : AbstractValidator<GetReportById>
{
	public GetReportByIdValidator()
	{
		RuleFor(x => x.Id)
			.NotEmpty();
	}
}