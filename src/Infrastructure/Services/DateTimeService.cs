using JourneyMate.Application.Common.Interfaces;

namespace JourneyMate.Infrastructure.Services;

internal sealed class DateTimeService : IDateTimeService
{
	public DateTime CurrentDate() => DateTime.UtcNow;
}