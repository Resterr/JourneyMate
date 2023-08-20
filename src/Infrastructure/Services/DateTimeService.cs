using JourneyMate.Application.Common.Interfaces;

namespace JourneyMate.Infrastructure.Services;

public class DateTimeService : IDateTimeService
{
    public DateTime Now => DateTime.UtcNow;
}
