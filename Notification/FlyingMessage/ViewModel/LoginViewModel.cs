namespace ILuffy.UGuest.Notification.ViewModel
{
    using System;
    using System.Security;
    using System.Windows.Input;
    using Domain;
    using Domain.Service;
    using IOP.Ioc;
    using IOP.UI.Input;
    using IOP.UI.ViewModel;
    using Repository;
    class LoginViewModel : WorkerViewModelBase, IDisposable
    {
        #region constructor

        public LoginViewModel()
        {
        }

        #endregion

        #region Properties
        private String userName;
        public String UserName
        {
            get
            {
                return userName;
            }
            set
            {
                CheckPropertyChanged(ref userName, value);
            }
        }

        private IHavePassword password;

        public IHavePassword Password
        {
            get
            {
                if (password == null)
                {
                    password = new PasswordProvider();
                }

                return password;
            }
            set
            {
                CheckPropertyChanged(ref password, value);
            }
        }

        private bool rememberPassword;
        public bool RememberPassword
        {
            get { return rememberPassword; }
            set { CheckPropertyChanged(ref rememberPassword, value); }
        }

        private ICommand loginCommand;
        public ICommand LoginCommand
        {
            get
            {
                if (loginCommand == null)
                {
                    loginCommand = new BackgroundWorkerCommand(
                        (sender, args) => Login(), 
                        param => CanLogin());
                }

                return loginCommand;
            }
        }

        private ICommand exitCommand;
        public ICommand ExistCommand
        {
            get { return exitCommand; }
            set { exitCommand = value; }
        }

        #endregion

        private void Login()
        {
            var loginService = IocContainer.Container.Resolve<ILoginService>();
            var config = IocContainer.Container.Resolve<IConfigRepository>();

            FlyingMessageContext.Current.HostUrl = config.HostUrl;
            FlyingMessageContext.Current.LoginName = userName;
            FlyingMessageContext.Current.Password = password.SecurePassword.Copy();

            if (loginService.Login())
            {
                Message = new MessageItem(MessageLevel.Info, "", "登陆。");
            }
            else
            {
                Message = new MessageItem(MessageLevel.Info, "", "登陆失败");
            }
        }

        private bool CanLogin()
        {
            return (!string.IsNullOrEmpty(userName)) && 
                (password != null && password.SecurePassword != null && password.SecurePassword.Length > 0);
        }

        public void Dispose()
        {
            if (password != null)
            {
                password.Dispose();
                password = null;
            }
        }
    }
}
