using System;

namespace BlogManagement.Infrastructure.Abstract
{
    public interface ISystemClock
    {
        DateTimeOffset GetCurrentDateTime();
    }
}