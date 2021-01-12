namespace Bloggo.Helpers
{
    public class PasswordHasher
    {
        private string _passwordHash;
        public PasswordHasher(string password) => _passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

        // public bool VerifyPassword(string enteredPassword, string passwordHash)
        // {
        //     return BCrypt.Net.BCrypt.Verify(enteredPassword, passwordHash);
        // }
        public string GetHash()
        {
            return _passwordHash;
        }

    }
}
