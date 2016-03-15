namespace ILuffy.UGuest.Repository
{
    using System;
    using System.Collections.Generic;
    using Domain;
    using I18N;
    using IOP;
    using IOP.Net;

    internal abstract class RepositoryBase
    {
        private const string loginUrlFormat = "{0}/_api/v1/login?name={1}&pwd={2}";

        public IJsonRequest JsonRequest { get; set; }

        protected bool Authenticate(bool throwExceptionIfLoginFailed)
        {
            var loginUrl = string.Format(loginUrlFormat,
                FlyingMessageContext.Current.HostUrl,
                FlyingMessageContext.Current.LoginName,
                FlyingMessageContext.Current.Password.ConvertToString());

            var token = JsonRequest.Get<Token>(new RequestParameters() { Url = loginUrl });

            if (!"UserLoginSuccessfully".Equals(token.ErrorCode, StringComparison.OrdinalIgnoreCase))
            {
                FlyingMessageContext.Current.Tokens = null;

                if (throwExceptionIfLoginFailed)
                {
                    throw new RepositoryException(
                        FlyingMessageRS.RepositoryRequestErrorMessageFormat(token.Message, token.ErrorCode),
                        ErrorCode.UnauthorizedAccess);

                }

                return false;
            }

            FlyingMessageContext.Current.Tokens = new Dictionary<string, string>()
            {
                { "ykyx", token.SessionId },
                { "X-XSRF-TOKEN", token.PostToken },
            };

            return true;
        }

        public T Get<T>(string url)
        {
            if (FlyingMessageContext.Current.Tokens == null)
            {
                Authenticate(true);
            }

            var obj = JsonRequest.Get<T>(new RequestParameters()
            {
                Url = url,
                Cookie = FlyingMessageContext.Current.Tokens,
            });

            return obj;
        }
    }
}
