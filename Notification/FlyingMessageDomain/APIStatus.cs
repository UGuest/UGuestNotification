namespace ILuffy.UGuest.Domain
{
    using System;

    public abstract class APIStatus
    {
        public string Message { get; set; }

        public string ErrorCode { get; set; }
    }
}
