namespace ILuffy.UGuest.Domain.Service
{
    using System;
    using IOP.Retry;
    using Repository;

    class LoginService : ILoginService
    {
        public ILoginRepository LoginRepository { get; set; }

        public IConfigRepository ConfigRepository { get; set; }

        public bool Login()
        {
            return RetryHelper.Retry(
                LoginRepository.Login,
                new LinearRetry(
                    ConfigRepository.RetryTimes,
                    new TimeSpan(0, 0, ConfigRepository.RetryInterval)));
        }
    }
}
