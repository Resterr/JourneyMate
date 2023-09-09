using JourneyMate.Application.Common.Interfaces;

namespace JourneyMate.Infrastructure.Services;
internal sealed class DateTimeService : IDateTimeService
{
    public DateTime Now => DateTime.UtcNow;
}
