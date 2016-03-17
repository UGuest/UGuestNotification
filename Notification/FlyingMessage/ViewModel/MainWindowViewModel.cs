namespace ILuffy.UGuest.Notification.ViewModel
{
    using System;
    using ILuffy.IOP.UI.ViewModel;
    using IOP.UI.Input;
    class MainWindowViewModel : ViewModelBase
    {
        private ViewModelBase content;
        public ViewModelBase Content
        {
            get
            {
                return content;
            }
            set
            {
                CheckPropertyChanged(ref content, value);
            }
        }


        public MainWindowViewModel()
        {
            var loginVM = new LoginViewModel();
            loginVM.ExistCommand = new RelayCommand(param => OnRequestClose());
            loginVM.AuthenticationCompleted += LoginVM_AuthenticationCompleted;

            content = loginVM;

            loginVM.LoginAutomatically();
        }

        private void LoginVM_AuthenticationCompleted(object sender, AuthenticationCompletedEventArgs e)
        {
            if (e.Authorised)
            {
                var originalVM = content as IDisposable;

                e.Context.Password = e.Context.Password.Copy();

                var allTradesVM = new AllTradesViewModel(e.Context);
                allTradesVM.ExistCommand = new RelayCommand(param => OnRequestClose());

                Content = allTradesVM;

                if (originalVM != null)
                {
                    originalVM.Dispose();
                }
            }
        }

        #region RequestClose [event]

        /// <summary>
        /// Raised when this workspace should be removed from the UI.
        /// </summary>
        public event EventHandler RequestClose;

        private void OnRequestClose()
        {
            EventHandler handler = this.RequestClose;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        #endregion // RequestClose [event]
    }
}
