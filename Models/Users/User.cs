using bank_app.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        private void setUserName(string name)
        {
            UserName = name;
        }

        private void setPassword(string password)
        {
            UserPassword = PasswordHasher.Hash(password);
        }

        private void setLoginAttempts(int attempts)
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

        private void setAccountStatus(AccountStatus accountStatus)
        {
            CurrentAccountStatus = accountStatus;
        }


        // public methods to access private setters

        public void updateUserInfo(string type, string change)
        {
            switch (type)
            {
                case "name":
                    setUserName(change);
                    break;

                case "password":
                    setPassword(change);
                    break;
            }
        }

        public void updateLoginAttempts(int change)
        {
            setLoginAttempts(change);
        }

        public void updateAccountStatus(AccountStatus newStatus)
        {
            setAccountStatus(newStatus);
        }
    }
}
