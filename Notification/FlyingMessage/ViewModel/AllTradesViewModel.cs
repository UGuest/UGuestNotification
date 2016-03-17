namespace ILuffy.UGuest.Notification.ViewModel
{
    using System;
    using System.Windows.Input;
    using Domain;
    using IOP.UI.Input;
    using IOP.UI.ViewModel;
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
            Message = new MessageItem(MessageLevel.Info, "Hello", "Test" + autoRefreshAllTrades + autoPrintTrade);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
