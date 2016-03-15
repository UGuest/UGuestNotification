namespace ILuffy.UGuest.Domain
{
    public enum TradeStatus
    {
        TRADE_NO_CREATE_PAY,
        WAIT_BUYER_PAY,
        WAIT_SELLER_SEND_GOODS,
        WAIT_BUYER_CONFIRM_GOODS,
        TRADE_BUYER_SIGNED,
        TRADE_CLOSED,
        TRADE_CLOSED_BY_USER
    }
}
