namespace Common.Application.Utils
{
    public static class PasswordHashUtil
    {
        public static string HashPassword(string password)
            => BCrypt.Net.BCrypt.EnhancedHashPassword(password);

        public static bool VerifyPassword(string password, string hashedPassword)
            => BCrypt.Net.BCrypt.EnhancedVerify(password, hashedPassword);
       
    }
}
