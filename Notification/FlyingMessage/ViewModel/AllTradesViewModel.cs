namespace ILuffy.UGuest.Notification.ViewModel
{
    using System;
    using Domain;
    using IOP.UI.ViewModel;
    class AllTradesViewModel : WorkerViewModelBase, IDisposable
    {
        private FlyingMessageContext context;

        public AllTradesViewModel(FlyingMessageContext context)
        {
            this.context = context;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
