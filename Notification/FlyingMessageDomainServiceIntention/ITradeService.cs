namespace ILuffy.UGuest.Domain.Service
{
    using System;
    using System.Collections.Generic;

    public interface ITradeService
    {
        DateTime LastQueryTime { get; set; }

        Trade[] GetAllTrades(QueryRule query, IList<string> filterOutTrades);

        void Update();
    }
}
