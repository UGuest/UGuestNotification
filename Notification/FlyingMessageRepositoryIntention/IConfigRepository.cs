namespace ILuffy.UGuest.Repository
{
    using System;
    using System.Collections.Generic;

    public interface IConfigRepository
    {
        string HostUrl { get; set; }

        string UserName { get; set; }

        string EncryptedPassword { get; set; }

        bool RememberAccount { get; set; }

        IList<string> PrintedTrades { get; set; }

        string PrinterEncodingName { get; set; }

        DateTime QueryTime { get; set; }

        int QueryInterval { get; set; }

        int RetryTimes { get; set; }

        int RetryInterval { get; set; }

        void Update();
    }
}
