namespace ILuffy.UGuest.Domain.Service
{
    using System;
    using System.Collections.Generic;
    using IOP.Printer;
    using Repository;

    internal class TradeService : ITradeService
    {
        public ITradeRepository TradeRepository { get; set; }

        public Trade[] GetAllTrades(QueryRule query, IList<string> filterOutTrades)
        {
            var allTrades = TradeRepository.GetAllTrades(query);

            if (allTrades != null && allTrades.Length > 0 && 
                filterOutTrades != null && filterOutTrades.Count > 0)
            {                
                var tempMapper = new Dictionary<string, Trade>();
                foreach(var item in allTrades)
                {
                    tempMapper[item.Id] = item; 
                }

                foreach(var item in filterOutTrades)
                {
                    tempMapper.Remove(item);
                }

                var values = tempMapper.Values;

                allTrades = new Trade[values.Count];
                values.CopyTo(allTrades, 0);
            }

            return allTrades;
        }
    }
}
