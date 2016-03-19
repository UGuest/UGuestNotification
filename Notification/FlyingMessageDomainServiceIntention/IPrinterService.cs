namespace ILuffy.UGuest.Domain.Service
{
    using System;
    using System.Collections.Generic;
    using IOP.Printer;
    public interface IPrinterService : IDisposable
    {
        IList<string> LastPrintedTrades { get; }

        bool InitializePrinter(PrinterParameter parameter);

        void Print(Trade trade);

        void Update();
    }
}
