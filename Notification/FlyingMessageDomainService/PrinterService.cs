namespace ILuffy.UGuest.Domain.Service
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
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

            printer = PrinterFactory.CreateInstance(parameter);

            return true;
        }

        public void Print(Trade trade)
        {
            if (printer == null)
            {
                throw new NullReferenceException("printer");
            }

            printer.WriteLine("订单号: {0}", trade.Id);
            printer.WriteLine("商品\t   规格\t   单价\t   数量");
            foreach(var item in trade.Orders)
            {
                var length = item.Title.Length * 2;
                if (item.Details != null)
                {
                    length += 6;
                    length += item.Details.Length * 2;
                }

                if(length >= 16)
                {
                    printer.ChangeAlignment(AlignmentType.Left);
                    printer.Write(item.Title);
                    if (!string.IsNullOrEmpty(item.Details))
                    {
                        printer.Write(" ");
                        printer.Write(item.Details);
                    }
                    printer.WriteLine();

                    printer.ChangeAlignment(AlignmentType.Right);
                    printer.Write(item.Price.ToString("C", CultureInfo.CreateSpecificCulture("zh-CN")));
                    printer.WriteLine("   x   {0}", item.Num);
                }
                else
                {
                    printer.ChangeAlignment(AlignmentType.Right);
                    printer.WriteLine("{0}   {1}\t{2}   x   {3}", 
                        item.Title, 
                        item.Details, 
                        item.Price.ToString("C", CultureInfo.CreateSpecificCulture("zh-CN")), 
                        item.Num);
                }
            }
            printer.ChangeAlignment(AlignmentType.Left);
            if (!string.IsNullOrEmpty(trade.BuyerMessage))
            {
                printer.WriteLine();
                printer.WriteLine("买家留言: {0}", trade.BuyerMessage);
            }

            printer.WriteLine();
            printer.WriteLine("合计: {0}", trade.Payment.ToString("C", CultureInfo.CreateSpecificCulture("zh-CN")));
            printer.WriteLine("付款方式: {0}", trade.PayTypeDisplayName);
            printer.WriteLine();
            printer.WriteLine();
            printer.WriteLine("收货人: {0}\t{1}", trade.ReceiverName, trade.ReceiverMobile);
            printer.WriteLine("地址: {0},{1},{2}", trade.ReceiverState, trade.ReceiverDistrict, trade.ReceiverAddress);
            printer.WriteLine("{0}", trade.ReceiverZip);
            printer.WriteLine("{0}", trade.Created.ToString("yyyy-MM-dd HH:mm:ss"));
            printer.WriteLine("================================");
            printer.WriteLine();
            printer.CutPaper();
            printer.Flush();

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

        public void Dispose()
        {
            if (printer != null)
            {
                printer.Dispose();
                printer = null;
            }
        }
    }
}
