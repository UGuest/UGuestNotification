namespace ILuffy.UGuest.Domain
{
    using System;

    public class Token : APIStatus
    {
        public string SessionId { get; set; }

        public string PostToken { get; set; }
    }
}
