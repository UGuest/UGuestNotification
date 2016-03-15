namespace ILuffy.UGuest.Domain
{
    public class QueryResult
    {
        public long Total { get; set; }

        public int CurrentPageNum { get; set; }

        public bool HasNext { get; set; }

        public Trade[] Trades { get; set; }
    }
}
