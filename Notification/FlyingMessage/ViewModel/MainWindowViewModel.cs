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
                if (content == null)
                {
                    var loginVM = new LoginViewModel();
                    loginVM.ExistCommand = new RelayCommand(param => OnRequestClose());
                    loginVM.AuthenticationCompleted += LoginVM_AuthenticationCompleted;

                    content = loginVM;
                }

                return content;
            }
            set
            {
                CheckPropertyChanged(ref content, value);
            }
        }

        private void LoginVM_AuthenticationCompleted(object sender, AuthenticationCompletedEventArgs e)
        {
            if (e.Authorised)
            {
                var originalVM = content;

                var allTradesVM = new AllTradesViewModel(e.Context);

                Content = allTradesVM;
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
