namespace ILuffy.UGuest.Notification.ViewModel
{
    using System;
    using Domain;
    delegate void AuthenticationCompletedEventHandler(object sender, AuthenticationCompletedEventArgs e);

    class AuthenticationCompletedEventArgs : EventArgs
    {
        private FlyingMessageContext context;
        private bool authorised;
        private Exception exception;

        public FlyingMessageContext Context { get { return context; } }

        public bool Authorised { get { return authorised; } }

        public Exception Exception { get { return exception; } }

        public AuthenticationCompletedEventArgs(FlyingMessageContext context, bool authorised, Exception exception)
        {
            this.context = context;
            this.authorised = authorised;
            this.exception = exception;
        }
    }
}
