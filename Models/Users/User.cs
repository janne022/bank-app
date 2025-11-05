using bank_app.Managers;
using bank_app.Utility;
using System.Text.RegularExpressions;

namespace bank_app.Models.Users
{
    public abstract class User
    {
        private readonly Regex _phoneReg = new Regex
            (
                @"^(?=(?:\D*\d){7,15}\D*$)\+?(\d{1,3})?[\s.-]?(?:(?:[2-9]\d{2})|(?:44\s?7\d{2})|(?:33\s?[67]\d{1})|(?:49\s?(?:1[5-7]\d))|
                (?:34\s?[67]\d)|(?:39\s?3[1-9]\d)|(?:31\s?6\d)|
                (?:46\s?7\d)|(?:353\s?8[1-9]\d))(?:[\s.-]?\d{2,4})+$"
            );

        private readonly Regex _emailReg = new Regex(@"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@([a-zA-Z0-9-]+\.)+[a-zA-Z]{2,}$");
        public string UserId { get; private set; }
        public string UserPassword { get; private set; }
        public string Email { get; private set; }
        public string PhoneNumber { get; private set; }
        public string LegalName { get; private set; }
        public int FailedLoginAttempts { get; private set; } = 0;
        public AccountStatus CurrentAccountStatus { get; private set; }
        public bool TwoFactorEnabled { get; private set; }
        private string? _twoFactorCode;
        public string TwoFactorCode
        {
            get
            {
                return _twoFactorCode;
            }

            set
            {
                if (value.Length == 6 && value.All(char.IsDigit))
                {
                    _twoFactorCode = value;
                }
                else
                {
                    throw new ArgumentException("must be 6 characters long and only be numbers");
                }
            }
        }

        public User(string userId, string userPassword, string email, string phoneNumber, string legalName, bool twoFactorEnabled = false)
        {
            UpdateUserId(userId);
            UserPassword = PasswordHasher.Hash(userPassword);
            SetEmail(email);
            SetPhoneNumber(phoneNumber);
            SetLegalName(legalName);
            CurrentAccountStatus = AccountStatus.Unlocked;
            TwoFactorEnabled = twoFactorEnabled;
        }

        // ---------------------------------------- PRIVATE SETTERS --------------------------------------- //
        private void SetLegalName(string name)
        {
            LegalName = name;
        }

        public void ClearTwoFactorCode()
        {
            _twoFactorCode = null;
        }

        private void SetUserId(string userId)
        {
            UserId = userId;
        }

        private void SetPassword(string password)
        {
            UserPassword = PasswordHasher.Hash(password);
        }

        private bool SetEmail(string email)
        {
            if (ValidEmail(email))
            {
                Email = email;
                return true;
            }
            return false;
        }

        private bool SetPhoneNumber(string phoneNumber)
        {
            if (ValidPhoneNumber(phoneNumber))
            {
                PhoneNumber = phoneNumber;
                return true;
            }
            return false;
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

        private bool ValidPhoneNumber(string number)
        {
            if (_phoneReg.IsMatch(number))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool ValidEmail(string email)
        {
            if (_emailReg.IsMatch(email))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // ----------------------------------------- PUBLIC UPDATING METHODS ----------------------------------------- //
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