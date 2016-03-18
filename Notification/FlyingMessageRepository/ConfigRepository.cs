namespace ILuffy.UGuest.Repository
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Configuration;

    internal class ConfigRepository : IConfigRepository
    {
        private Configuration config;
        private KeyValueConfigurationCollection settings;
        private bool changed;

        public ConfigRepository()
        {
            config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            settings = config.AppSettings.Settings;
            changed = false;
        }

        private string GetValue(string key)
        {
            var valueElement = settings[key];

            if (valueElement != null)
            {
                return valueElement.Value;
            }

            return null;
        }

        private void SetValue(string key, string value)
        {
            var settings = config.AppSettings.Settings;

            if (settings[key] == null)
            {
                settings.Add(key, value);
            }
            else
            {
                settings[key].Value = value;
            }
            changed = true;
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

        private bool GetBoolean(string key)
        {
            var value = false;

            var boolAsString = GetValue(key);

            bool.TryParse(boolAsString, out value);

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

        public string PrinterEncodingName
        {
            get { return GetValue("printerEncodingName"); }
            set { SetValue("printerEncodingName", value); }
        }

        public DateTime QueryTime
        {
            get { return GetDateTime("queryTime"); }
            set { SetValue("queryTime", value.ToDefaultString()); }
        }

        public int QueryInterval
        {
            get { return GetInt32("queryInterval"); }
            set { SetValue("queryInterval", value.ToString()); }
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

        public string UserName
        {
            get { return GetValue("userName"); }
            set { SetValue("userName", value); }
        }

        public string EncryptedPassword
        {
            get { return GetValue("encryptedPassword"); }
            set { SetValue("encryptedPassword", value); }
        }

        public bool RememberAccount
        {
            get { return GetBoolean("rememberAccount"); }
            set { SetValue("rememberAccount", value.ToString()); }
        }


        public void Update()
        {
            if (changed)
            {
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(config.AppSettings.SectionInformation.Name);
            }
        }
    }
}
