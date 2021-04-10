// PasswordHelper.cs

namespace MarkPad.Server
{
    public class PasswordHelper
    {
        public static string GetHash(string password)
        {
            byte[] salt;
            new System.Security.Cryptography.RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);

            System.Security.Cryptography.Rfc2898DeriveBytes pbkdf2 = new System.Security.Cryptography.Rfc2898DeriveBytes(password, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(20);

            byte[] hashBytes = new byte[36];
            System.Array.Copy(salt, 0, hashBytes, 0, 16);
            System.Array.Copy(hash, 0, hashBytes, 16, 20);

            return System.Convert.ToBase64String(hashBytes);
        }

        public static bool Check(string password, string passwordHash)
        {
            byte[] hashBytes = System.Convert.FromBase64String(passwordHash);

            byte[] salt = new byte[16];
            System.Array.Copy(hashBytes, 0, salt, 0, 16);

            System.Security.Cryptography.Rfc2898DeriveBytes pbkdf2 = new System.Security.Cryptography.Rfc2898DeriveBytes(password, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(20);

            for (int i = 0; i < 20; ++i)
            {
                if (hashBytes[i + 16] != hash[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}
