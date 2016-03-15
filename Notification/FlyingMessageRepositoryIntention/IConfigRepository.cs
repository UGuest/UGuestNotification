namespace ILuffy.UGuest.Repository
{
    using System;
    using System.Collections.Generic;

    public interface IConfigRepository
    {
        string HostUrl { get; set; }

        IList<string> PrintedTrades { get; set; }

        DateTime QueryTime { get; set; }

        int RetryTimes { get; set; }

        int RetryInterval { get; set; }
    }
}
