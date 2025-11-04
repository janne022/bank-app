using bank_app.Managers;
using bank_app.Utility;

namespace bank_app.Models.Users
{
    public abstract class User
    {
        public string UserId { get; private set; }
        public string UserPassword { get; private set; }
        public string Email { get; private set; }
        public string PhoneNumber { get; private set; }
        public string LegalName { get; private set; }
        public int FailedLoginAttempts { get; private set; } = 0;
        public AccountStatus CurrentAccountStatus { get; private set; }

        public User(string userId, string userPassword, string email, string phoneNumber, string legalName)
        {
            UpdateUserId(userId);
            UserPassword = PasswordHasher.Hash(userPassword);
            SetEmail(email);
            SetPhoneNumber(phoneNumber);
            SetLegalName(legalName);
            CurrentAccountStatus = AccountStatus.Unlocked;
        }

        // ----------------------------------------private setters--------------------------------------- //
        private void SetLegalName(string name)
        {
            LegalName = name;
        }

        private void SetUserId(string userId)
        {
            UserId = userId;
        }

        private void SetPassword(string password)
        {
            UserPassword = PasswordHasher.Hash(password);
        }

        private void SetEmail(string email)
        {
            Email = email;
        }

        private void SetPhoneNumber(string phoneNumber)
        {
            PhoneNumber = phoneNumber;
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
        public bool UpdateUserId(string userId)
        {
            if (UserManager.UserIds.Contains(userId))
            {
                return false;
            }
            else
            {
                UserManager.UserIds.Remove(UserId);
                SetUserId(userId);
                UserManager.UserIds.Add(UserId);
                return true;
            }
        }

        public void UpdateUserInfo(string type, string change)
        {
            switch (type)
            {
                case "id":
                    UpdateUserId(change);
                    break;

                case "password":
                    SetPassword(change);
                    break;

                case "email":
                    SetEmail(change);
                    break;

                case "number":
                    SetPhoneNumber(change);
                    break;

                case "name":
                    SetLegalName(change);
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
