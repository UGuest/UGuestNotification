namespace ILuffy.UGuest.Domain.Service
{
    using System;

    public interface ICryptographyService
    {
        string Protect(string plainText);
        string Unprotect(string encryptedText);
    }
}
