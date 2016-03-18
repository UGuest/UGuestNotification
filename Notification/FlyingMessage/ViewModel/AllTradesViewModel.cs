namespace ILuffy.UGuest.Notification.ViewModel
{
    using System;
    using System.Threading;
    using System.Windows.Input;
    using Domain;
    using Domain.Service;
    using IOP.Ioc;
    using IOP.Printer;
    using IOP.UI.Input;
    using IOP.UI.ViewModel;
    using Repository;
    class AllTradesViewModel : WorkerViewModelBase, IDisposable
    {
        private FlyingMessageContext context;

        #region Properties

        private bool autoRefreshAllTrades;
        public bool AutoRefreshAllTrades
        {
            get { return autoRefreshAllTrades; }
            set { CheckPropertyChanged(ref autoRefreshAllTrades, value); }
        }

        private bool autoPrintTrade;
        public bool AutoPrintTrade
        {
            get { return autoPrintTrade; }
            set { CheckPropertyChanged(ref autoPrintTrade, value); }
        }

        private ICommand startQueryCommand;
        public ICommand StartQueryCommand
        {
            get
            {
                if (startQueryCommand == null)
                {
                    startQueryCommand = new BackgroundWorkerCommand((sender, args) => QueryAndPrint());
                }

                return startQueryCommand;
            }
        }

        private ICommand stopQueryCommand;
        public ICommand StopQueryCommand
        {
            get
            {
                return stopQueryCommand;
            }
        }

        private ICommand reLoginCommand;
        public ICommand ReLoginCommand
        {
            get { return reLoginCommand; }
        }

        private ICommand existCommand;
        public ICommand ExistCommand
        {
            get { return existCommand; }
            set { existCommand = value; }
        }

        #endregion


        public AllTradesViewModel(FlyingMessageContext context)
        {
            this.context = context;
        }

        private void QueryAndPrint()
        {
            var configRepository = IocContainer.Container.Resolve<IConfigRepository>();
            var tradeService = IocContainer.Container.Resolve<ITradeService>();
            var printerService = IocContainer.Container.Resolve<IPrinterService>();

            printerService.InitializePrinter(new PrinterParameter()
            {
                Type = PrinterType.USB,
                EncodingName =configRepository.PrinterEncodingName
            });

            var queryRule = new QueryRule()
            {
                PayType = PayTypeLite.All,
                Status = TradeStatusLite.ToSend,
                EndTime =DateTime.MinValue
            };


            while (true)
            {
                queryRule.StartTime = configRepository.QueryTime;

                var trades = tradeService.GetAllTrades(queryRule, printerService.LastPrintedTrades);

                if (trades != null && trades.Length > 0)
                {
                    Message = new MessageItem(MessageLevel.Info, null, "trades.count:" + trades.Length);

                    foreach (var item in trades)
                    {
                        printerService.Print(item);

                        if (item.Created > queryRule.StartTime)
                        {
                            queryRule.StartTime = item.Created;
                        }
                    }

                    printerService.Update();

                    configRepository.QueryTime = queryRule.StartTime;
                    configRepository.Update();
                }
                else
                {
                    Thread.Sleep(configRepository.QueryInterval * 1000);
                }
            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
