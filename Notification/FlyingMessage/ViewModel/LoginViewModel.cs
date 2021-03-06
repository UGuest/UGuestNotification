﻿namespace ILuffy.UGuest.Notification.ViewModel
{
    using System;
    using System.Security;
    using System.Windows.Input;
    using Domain;
    using Domain.Service;
    using IOP;
    using IOP.Ioc;
    using IOP.UI;
    using IOP.UI.Input;
    using IOP.UI.ViewModel;
    using Repository;

    class LoginViewModel : WorkerViewModelBase, IDisposable
    {
        #region constructor

        public LoginViewModel()
        {
            rememberPassword = true;
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
                        (sender, args) => Login(true), 
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

        #region Events
        private AuthenticationCompletedEventHandler authenticationCompleted;
        public event AuthenticationCompletedEventHandler AuthenticationCompleted
        {
            add
            {
                authenticationCompleted = 
                    (AuthenticationCompletedEventHandler)Delegate.Combine(authenticationCompleted, value);
            }
            remove
            {
                authenticationCompleted = 
                    (AuthenticationCompletedEventHandler)Delegate.Remove(authenticationCompleted, value);
            }
        }

        private void OnAuthenticationCompleted(AuthenticationCompletedEventArgs e)
        {
            if (authenticationCompleted != null)
            {
                authenticationCompleted(this, e);
            }
        }
        #endregion

        public void LoginAutomatically()
        {
            try
            {
                var cryptographyService = IocContainer.Container.Resolve<ICryptographyService>();
                var config = IocContainer.Container.Resolve<IConfigRepository>();

                UserName = config.UserName;
                Password.SecurePassword = cryptographyService.Unprotect(config.EncryptedPassword).
                    ConvertToSecureString();
                RememberPassword = config.RememberAccount;

                if (RememberPassword && Password.SecurePassword.Length > 0)
                {
                    Login(false);
                }
            }
            catch(Exception ex)
            {
                LoggerUtility.WriteMessage(Severity.Error,
                    I18NUtility.GetString("I18N_LoginFailedWithDetails"), ex);

                Message = new MessageItem(MessageLevel.Error,
                        I18NUtility.GetString("I18N_MessageBoxTitle"),
                        I18NUtility.GetString("I18N_LoginFailedWithDetails", ex.Message));
            }
        }

        private void Login(bool saveCredential)
        {
            FlyingMessageContext context = null;
            var authorised = false;
            Exception exception = null;

            try
            {
                var loginService = IocContainer.Container.Resolve<ILoginService>();
                var config = IocContainer.Container.Resolve<IConfigRepository>();

                FlyingMessageContext.Current.HostUrl = config.HostUrl;
                FlyingMessageContext.Current.LoginName = userName;
                FlyingMessageContext.Current.Password = password.SecurePassword;

                context = FlyingMessageContext.Current;

                if (loginService.Login())
                {
                    if (saveCredential)
                    {
                        config.UserName = userName;
                        config.RememberAccount = rememberPassword;
                        if (rememberPassword)
                        {
                            var cryptographyService = IocContainer.Container.Resolve<ICryptographyService>();
                            config.EncryptedPassword = cryptographyService.Protect(
                                password.SecurePassword.ConvertToString());
                            config.RememberAccount = rememberPassword;
                        }
                        else
                        {
                            config.EncryptedPassword = string.Empty;
                        }
                        config.Update();
                    }
                    authorised = true;
                }
                else
                {
                    Message = new MessageItem(MessageLevel.Error,
                        I18NUtility.GetString("I18N_MessageBoxTitle"),
                        I18NUtility.GetString("I18N_LoginFailed"));
                }
            }
            catch(Exception ex)
            {
                exception = ex;
                LoggerUtility.WriteMessage(Severity.Error, 
                    I18NUtility.GetString("I18N_LoginFailedWithDetails"), ex);

                Message = new MessageItem(MessageLevel.Error,
                        I18NUtility.GetString("I18N_MessageBoxTitle"),
                        I18NUtility.GetString("I18N_LoginFailedWithDetails", ex.Message));
            }
            finally
            {
                FlyingMessageContext.Current = null;
            }

            var eventArgs = new AuthenticationCompletedEventArgs(context, authorised, exception);
            OnAuthenticationCompleted(eventArgs);
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

            if (loginCommand != null)
            {
                var disposable = loginCommand as IDisposable;

                if (disposable != null)
                {
                    disposable.Dispose();
                }
            }
        }
    }
}
