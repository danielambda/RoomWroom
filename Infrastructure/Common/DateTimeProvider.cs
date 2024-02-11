using Application.Common.Interfaces;

namespace Infrastructure.Common;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime Now => DateTime.Now;
}