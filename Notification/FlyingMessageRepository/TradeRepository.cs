namespace ILuffy.UGuest.Repository
{
    using System;
    using System.Collections.Generic;
    using ILuffy.UGuest.Domain;

    internal class TradeRepository : RepositoryBase, ITradeRepository
    {
        /// <summary>
        /// http://xxxx/_api/v1/trade.get?startTime=2015-01-01&endTime=2015-07-01&status=toSend&payType=all
        /// </summary>
        private const string queryTrade = 
            "{0}/_api/v1/trade.get?startTime={0}&endTime={1}&status={2}&payType={3}&page={4}";

        public Trade[] GetAllTrades(QueryRule query)
        {
            var allTrades = new List<Trade>();

            var pageQuery = new QueryRuleWithPage()
            {
                StartTime = query.StartTime,
                EndTime = query.EndTime,
                PayType = query.PayType,
                Status = query.Status,
                Page = 1,
            };

            while (true)
            {
                var queryResult = GetTrades(pageQuery);

                allTrades.AddRange(queryResult.Trades);

                if (queryResult.HasNext)
                {
                    pageQuery.Page = pageQuery.Page + 1;
                }
                else
                {
                    break;
                }
            }

            return allTrades.ToArray();
        }

        public QueryResult GetTrades(QueryRuleWithPage query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }

            var context = FlyingMessageContext.Current;

            var requestUrl = string.Format(queryTrade, context.HostUrl, 
                query.StartTime.ToDefaultString(), 
                query.EndTime.ToDefaultString(), 
                query.Status.ToString("F"), 
                query.PayType.ToString("F"), 
                query.Page);

            return Get<QueryResult>(requestUrl);
        }
    }
}
