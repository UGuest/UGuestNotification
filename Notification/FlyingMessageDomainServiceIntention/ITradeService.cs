namespace ILuffy.UGuest.Domain.Service
{
    using System;
    using System.Collections.Generic;

    public interface ITradeService
    {
        Trade[] GetAllTrades(QueryRule query, IList<string> filterOutTrades);
    }
}
