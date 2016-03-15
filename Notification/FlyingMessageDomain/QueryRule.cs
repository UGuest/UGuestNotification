namespace ILuffy.UGuest.Domain
{
    using System;

    public class QueryRule
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public TradeStatusLite Status { get; set; }
        public PayTypeLite PayType { get; set; }
    }
}
