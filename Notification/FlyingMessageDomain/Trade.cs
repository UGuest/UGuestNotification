namespace ILuffy.UGuest.Domain
{
    using System;

    public class Trade
    {
        public string Id { get; set; }

        public Order[] Orders { get; set; }

        public string BuyerNick { get; set; }

        public string BuyerMessage { get; set; }

        public string ReceiverName { get; set; }

        public string ReceiverMobile { get; set; }

        public string ReceiverState { get; set; }

        public string ReceiverDistrict { get; set; }

        public string ReceiverAddress { get; set; }

        public string ReceiverZip { get; set; }

        public DateTime Created { get; set; }

        public TradeStatus Status { get; set; }

        public string StatusDisplayName { get; set; }

        public double Payment { get; set; }

        public double PostFee { get; set; }

        public PayType PayType { get; set; }

        public string PayTypeDisplayName { get; set; }
    }
}
