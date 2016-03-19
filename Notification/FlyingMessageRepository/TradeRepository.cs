namespace ILuffy.UGuest.Repository
{
    using System;
    using System.Collections.Generic;
    using I18N;
    using ILuffy.UGuest.Domain;

    internal class TradeRepository : RepositoryBase, ITradeRepository
    {
        /// <summary>
        /// http://xxxx/_api/v1/trade.get?startTime=2015-01-01&endTime=2015-07-01&status=toSend&payType=all
        /// </summary>
        private const string queryTrade = 
            "{0}/_api/v1/trade.get?status={1}&payType={2}&page={3}";

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

                if (queryResult.Trades != null)
                {
                    allTrades.AddRange(queryResult.Trades);
                }

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
                query.Status.ToString("F"), 
                query.PayType.ToString("F"), 
                query.Page);

            if (query.StartTime != DateTime.MinValue)
            {
                requestUrl = string.Format("{0}&startTime={1}", requestUrl, query.StartTime.ToDefaultString());
            }

            if (query.EndTime != DateTime.MinValue)
            {
                requestUrl = string.Format("{0}&endTime={1}", requestUrl, query.EndTime.ToDefaultString());
            }

            var resultWrapper = Get<QueryResultWrapper>(requestUrl);

            if (resultWrapper.QueryResult != null)
            {
                return resultWrapper.QueryResult;
            }

            throw new RepositoryException(
                        FlyingMessageRS.RepositoryRequestErrorMessageFormat(
                            resultWrapper.Message, 
                            resultWrapper.ErrorCode));
        }
    }
}
