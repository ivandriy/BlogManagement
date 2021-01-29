using BlogManagement.Infrastructure.Abstract;
using System;

namespace BlogManagement.Infrastructure
{
    public class SystemClock : ISystemClock
    {
        public DateTimeOffset GetCurrentDateTime() => DateTimeOffset.Now;
    }
}