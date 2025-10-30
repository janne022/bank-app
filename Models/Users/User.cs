using bank_app.Utility;

namespace bank_app.Models.Users
{
    public abstract class User
    {
        internal Guid UserId { get; private set; } = Guid.NewGuid();
        internal string? UserName { get; set; }
        internal string? UserPassword { get; set; }
        internal int FailedLoginAttempts { get; set; } = 0;
        internal AccountStatus CurrentAccountStatus { get; set; }

        protected User(string userName, string userPassword)
        {
            UserName = userName;
            UserPassword = PasswordHasher.Hash(userPassword);
        }
    }
}
