using AutoMapper;
using AutoMapper.QueryableExtensions;
using JourneyMate.Application.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace JourneyMate.Application.Common.Mappings;

public static class MappingExtensions
{
	public static Task<PaginatedList<TDestination>> PaginatedListAsync<TDestination>(this IQueryable<TDestination> queryable, int pageNumber, int pageSize) where TDestination : class
	{
		return PaginatedList<TDestination>.CreateAsync(queryable.AsNoTracking(), pageNumber, pageSize);
	}

	public static Task<List<TDestination>> ProjectToListAsync<TDestination>(this IQueryable queryable, IConfigurationProvider configuration) where TDestination : class
	{
		return queryable.ProjectTo<TDestination>(configuration)
			.AsNoTracking()
			.ToListAsync();
	}
}