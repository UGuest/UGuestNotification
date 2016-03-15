namespace ILuffy.UGuest.Repository
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Configuration;

    internal class ConfigRepository : IConfigRepository
    {
        private string GetValue(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        private void SetValue(string key, string value)
        {
            ConfigurationManager.AppSettings.Set(key, value);
        }

        private DateTime GetDateTime(string key)
        {
            var timeAsString = GetValue(key);

            return timeAsString.ToDataTimeWithDefaultStyle();
        }

        private int GetInt32(string key)
        {
            int value = 0;

            var int32AsString = GetValue(key);

            int.TryParse(int32AsString, out value);

            return value;
        }

        public IList<string> PrintedTrades
        {
            get
            {
                var trades = GetValue("printedTrades");

                if (trades != null)
                {
                    return trades.Split(';').ToList();
                }

                return null;
            }
            set
            {
                string trades = null;

                if(value != null)
                {
                    trades = value.Aggregate((a, b) => a + ';' + b);
                }

                SetValue("printedTrades", trades);
            }
        }

        public DateTime QueryTime
        {
            get { return GetDateTime("queryTime"); }
            set { SetValue("queryTime", value.ToDefaultString()); }
        }

        public int RetryTimes
        {
            get { return GetInt32("retryTimes"); }
            set { SetValue("retryTimes", value.ToString()); }
        }

        public int RetryInterval
        {
            get { return GetInt32("retryInterval"); }
            set { SetValue("retryInterval", value.ToString()); }
        }

        public string HostUrl
        {
            get { return GetValue("hostUrl"); }
            set { SetValue("hostUrl", value); }
        }
    }
}
