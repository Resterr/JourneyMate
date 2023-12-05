using AutoMapper;
using FluentValidation;
using JourneyMate.Application.Common.Exceptions;
using JourneyMate.Application.Common.Interfaces;
using JourneyMate.Application.Common.Mappings;
using JourneyMate.Application.Common.Models;
using JourneyMate.Domain.Entities.MongoDb;
using JourneyMate.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;

namespace JourneyMate.Application.Features.PlaceFeature.Queries;

public record GetAllReportsPaginated(int PageNumber, int PageSize) : IRequest<PaginatedList<ReportDto>>;

internal sealed class GetAllReportsHandler : IRequestHandler<GetAllReportsPaginated, PaginatedList<ReportDto>>
{
	private readonly IApplicationDbContext _dbContext;
	private readonly IApplicationMongoClient _mongoClient;
	private readonly ICurrentUserService _currentUserService;
	private readonly IMapper _mapper;

	public GetAllReportsHandler(IApplicationDbContext dbContext, IApplicationMongoClient mongoClient, ICurrentUserService currentUserService, IMapper mapper)
	{
		_dbContext = dbContext;
		_mongoClient = mongoClient;
		_currentUserService = currentUserService;
		_mapper = mapper;
	}

	public async Task<PaginatedList<ReportDto>> Handle(GetAllReportsPaginated request, CancellationToken cancellationToken)
	{
		var userId = _currentUserService.UserId ?? throw new UnauthorizedAccessException();
		var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId) ?? throw new UserNotFoundException(userId);
		var filter = Builders<Report>.Filter.Eq(x => x.UserId, user.Id);
		var reports = await _mongoClient.Reports.Find(filter)
			.ToListAsync();

		var reportsPaginated = reports.AsQueryable().OrderByDescending(x => x.Created).PaginatedListSync(request.PageNumber, request.PageSize);
		var result = _mapper.Map<List<ReportDto>>(reportsPaginated.Items);

		return new PaginatedList<ReportDto>(result, reportsPaginated.TotalCount, request.PageNumber, request.PageSize);
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