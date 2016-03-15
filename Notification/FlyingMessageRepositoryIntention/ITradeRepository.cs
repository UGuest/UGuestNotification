namespace ILuffy.UGuest.Repository
{
    using ILuffy.UGuest.Domain;

    public interface ITradeRepository
    {
        Trade[] GetAllTrades(QueryRule query);

        QueryResult GetTrades(QueryRuleWithPage query);
    }
}
