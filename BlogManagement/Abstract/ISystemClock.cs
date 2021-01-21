using System;

namespace BlogManagement.Abstract
{
    public interface ISystemClock
    {
        DateTimeOffset GetCurrentDateTime();
    }
}