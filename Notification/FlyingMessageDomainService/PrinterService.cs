namespace ILuffy.UGuest.Domain.Service
{
    using System;
    using System.Collections.Generic;
    using IOP.Printer;
    using Repository;

    class PrinterService : IPrinterService
    {
        private IPrinter printer;
        private List<string> printedTrades;

        public IConfigRepository ConfigRepository { get; set; }

        public IList<string> LastPrintedTrades
        {
            get
            {
                return ConfigRepository.PrintedTrades;
            }
        }

        public bool InitializePrinter(PrinterParameter parameter)
        {
            if (printer != null)
            {
                printer.Dispose();
            }

            printer = parameter.CreateInstance<IPrinter>(parameter);

            return true;
        }

        public void Print(Trade trade)
        {
            if (printer == null)
            {
                throw new NullReferenceException("printer");
            }

            printer.WriteLine("交易单号:\t{0}", trade.Id);
            printer.WriteLine("商品\t规格\t单价\t数量");
            foreach(var item in trade.Orders)
            {
                printer.WriteLine("{0}\t{1}\t{2}\t{3}", item.Title, item.Details, item.Price, item.Num);
            }

            if (!string.IsNullOrEmpty(trade.BuyerMessage))
            {
                printer.WriteLine();
                printer.WriteLine("买家留言:\t{0}", trade.BuyerMessage);
            }

            printer.WriteLine();
            printer.WriteLine("总  计:\t{0}", trade.Payment);
            printer.WriteLine("付款方式:\t{0}", trade.PayTypeDisplayName);
            printer.WriteLine();
            printer.WriteLine();
            printer.WriteLine("{0}\t{1}", trade.ReceiverName, trade.ReceiverMobile);
            printer.WriteLine("{0},{1},{2}", trade.ReceiverState, trade.ReceiverDistrict, trade.ReceiverAddress);
            printer.WriteLine("{0}", trade.ReceiverZip);
            printer.WriteLine("{0}", trade.Created.ToString("yyyy-MM-dd HH:mm:ss"));

            printer.WriteLine();
            printer.CutPaper();

            OnPrintedTrade(trade);
        }

        private void OnPrintedTrade(Trade trade)
        {
            if (printedTrades == null)
            {
                printedTrades = new List<string>();
            }

            printedTrades.Add(trade.Id);
        }

        public void Update()
        {
            ConfigRepository.PrintedTrades = printedTrades;

            printedTrades = null;
        }
    }
}
