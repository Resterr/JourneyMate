using AutoMapper;
using JourneyMate.Application.Common.Exceptions;
using JourneyMate.Application.Common.Interfaces;
using JourneyMate.Application.Common.Models;
using JourneyMate.Domain.Entities;
using JourneyMate.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace JourneyMate.Application.Features.PlaceFeature.Queries;

public record GetAllReports : IRequest<List<ReportListDto>>;

internal sealed class GetAllReportsHandler : IRequestHandler<GetAllReports, List<ReportListDto>>
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

	public async Task<List<ReportListDto>> Handle(GetAllReports request, CancellationToken cancellationToken)
	{
		var userId = _currentUserService.UserId ?? throw new UnauthorizedAccessException();
		var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId) ?? throw new UserNotFoundException(userId);
		var filter = Builders<Report>.Filter.Eq(x => x.UserId, user.Id);
		var reports = await _mongoClient.Reports.Find(filter)
			.ToListAsync();

		var result = reports.Select(report => new ReportListDto
		{
			Id = report.Id,
			AddressId = report.AddressId,
			Rating = report.Rating,
			Places = report.Places,
			Types = report.Types
		}).ToList();

		return result;
	}
}