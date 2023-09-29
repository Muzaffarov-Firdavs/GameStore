namespace GameStore.Service.Commons.Security
{
    public class PasswordHasher
    {
        public static (string HashedPassword, string Salt) Hash(string password)
        {
            string salt = GenerateSalt();
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(salt + password);
            return (HashedPassword: hashedPassword, Salt: salt);
        }

        public static bool Verify(string password, string salt, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(salt + password, hash);
        }

        public static string ChangePassword(string password, string salt)
        {
            return BCrypt.Net.BCrypt.HashPassword(salt + password);
        }

        private static string GenerateSalt()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
