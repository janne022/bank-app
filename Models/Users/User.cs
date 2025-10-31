using bank_app.Utility;

namespace bank_app.Models.Users
{
    public abstract class User
    {
        public Guid UserId { get; private set; } = Guid.NewGuid();
        public string? UserName { get; private set; }
        public string? UserPassword { get; private set; }
        public int FailedLoginAttempts { get; private set; } = 0;
        public AccountStatus CurrentAccountStatus { get; private set; }

        public User(string userName, string userPassword)
        {
            UserName = userName;
            UserPassword = PasswordHasher.Hash(userPassword);
        }


        // private setters
        private void SetUserName(string name)
        {
            UserName = name;
        }

        private void SetPassword(string password)
        {
            UserPassword = PasswordHasher.Hash(password);
        }

        private void SetLoginAttempts(int attempts)
        {
            if (attempts == 1)
            {
                FailedLoginAttempts += 1;
            }
            else if (attempts == 0)
            {
                FailedLoginAttempts = 0;
            }

        }

        private void SetAccountStatus(AccountStatus accountStatus)
        {
            CurrentAccountStatus = accountStatus;
        }


        // public methods to access private setters

        public void UpdateUserInfo(string type, string change)
        {
            switch (type)
            {
                case "name":
                    SetUserName(change);
                    break;

                case "password":
                    SetPassword(change);
                    break;
            }
        }

        public void UpdateLoginAttempts(int change)
        {
            SetLoginAttempts(change);
        }

        public void UpdateAccountStatus(AccountStatus newStatus)
        {
            SetAccountStatus(newStatus);
        }
    }
}
