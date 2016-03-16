namespace ILuffy.UGuest.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Security;
    using System.Threading;

    public class FlyingMessageContext
    {
        private static object syncObj = new object();

        protected static ThreadLocal<FlyingMessageContext> instance;

        public static FlyingMessageContext Current
        {
            get
            {
                if (instance == null || instance.Value == null)
                {
                    lock (syncObj)
                    {
                        if (instance == null)
                        {
                            instance = new ThreadLocal<FlyingMessageContext>();
                        }

                        if (instance.Value == null)
                        {
                            instance.Value = new FlyingMessageContext();
                        }
                    }
                }

                return instance.Value;
            }
            set
            {
                if (instance != null)
                {
                    instance.Value = value;
                }
            }
        }

        public string LoginName { get; set; }

        public SecureString Password { get; set; }

        public string HostUrl { get; set; }

        public Dictionary<string,string> Tokens { get; set; }
    }
}
