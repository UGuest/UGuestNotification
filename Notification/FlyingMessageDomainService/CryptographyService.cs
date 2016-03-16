namespace ILuffy.UGuest.Domain.Service
{
    using System;
    using System.Security.Cryptography;
    using System.Text;
    class CryptographyService : ICryptographyService
    {
        public string Protect(string plainText)
        {
            string encryptedString = null;

            if (!string.IsNullOrEmpty(plainText))
            {
                var bytes = Encoding.UTF8.GetBytes(plainText);
                var protectedBytes = ProtectedData.Protect(bytes, null, DataProtectionScope.LocalMachine);
                encryptedString = Convert.ToBase64String(protectedBytes);
            }

            return encryptedString;
        }

        public string Unprotect(string encryptedText)
        {
            string plainText = null;

            if (!string.IsNullOrEmpty(encryptedText))
            {
                var protectedBytes = Convert.FromBase64String(encryptedText);
                var bytes = ProtectedData.Unprotect(protectedBytes, null, DataProtectionScope.LocalMachine);
                plainText = Encoding.UTF8.GetString(bytes);
            }

            return plainText;
        }
    }
}
