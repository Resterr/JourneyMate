using AutoMapper;
using FluentValidation;
using JourneyMate.Application.Common.Exceptions;
using JourneyMate.Application.Common.Interfaces;
using JourneyMate.Application.Common.Models;
using JourneyMate.Domain.Entities;
using JourneyMate.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace JourneyMate.Application.Features.PlaceFeature.Queries;

public record GetReportById(Guid Id) : IRequest<ReportDto>;

internal sealed class GetReportByIdHandler : IRequestHandler<GetReportById, ReportDto>
{
	private readonly IApplicationDbContext _dbContext;
	private readonly IApplicationMongoClient _mongoClient;
	private readonly ICurrentUserService _currentUserService;
	private readonly IMapper _mapper;

	public GetReportByIdHandler(IApplicationDbContext dbContext, IApplicationMongoClient mongoClient, ICurrentUserService currentUserService, IMapper mapper)
	{
		_dbContext = dbContext;
		_mongoClient = mongoClient;
		_currentUserService = currentUserService;
		_mapper = mapper;
	}

	public async Task<ReportDto> Handle(GetReportById request, CancellationToken cancellationToken)
	{
		var userId = _currentUserService.UserId ?? throw new UnauthorizedAccessException();
		var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId) ?? throw new UserNotFoundException(userId);
		var filter = Builders<Report>.Filter.Eq(x => x.Id, request.Id) & Builders<Report>.Filter.Eq(x => x.UserId, user.Id);
		var report = await _mongoClient.Reports.Find(filter)
			.FirstOrDefaultAsync() ?? throw new ReportNotFound(request.Id);

		var placesDto = new List<PlaceDto>();
		if (report.Places.Count > 0)
		{
			foreach (var placeDtoId in report.Places)
			{
				var place = await _dbContext.Places.Include(x => x.Addresses).Include(x => x.Types).FirstOrDefaultAsync(x => x.Id == placeDtoId);

				if (place != null)
				{
					placesDto.Add(_mapper.Map<PlaceDto>(place));
				}
			}
		}

		var result = new ReportDto()
		{
			Id = report.Id,
			Rating = report.Rating,
			Places = placesDto,
			Types = report.Types
		};
		
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