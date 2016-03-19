namespace ILuffy.UGuest.Domain
{
    using System;
    using Newtonsoft.Json;
    public class Token : APIStatus
    {
        [JsonProperty("ykyx")]
        public string SessionId { get; set; }

        [JsonProperty("X-XSRF-TOKEN")]
        public string PostToken { get; set; }
    }
}
