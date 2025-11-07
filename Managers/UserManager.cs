using bank_app.Models.Users;
using bank_app.Utility;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace bank_app.Managers
{
    public static class UserManager
    {
        public static List<User> Users { get; } = new List<User>();
        public static List<string> UserIds { get; } = new List<string>();

        internal static User CreateUser(string userId, string userPassword, UserType userType, string email, string phoneNumber, string legalName, bool twoFactorEnabled = false)
        {
            if (userType is UserType.Admin)
            {
                var admin = new Admin(userId, userPassword, email, phoneNumber, legalName, twoFactorEnabled);
                Users.Add(admin);
                return admin;
            }
            else if (userType is UserType.Client)
            {
                var client = new Client(userId, userPassword, email, phoneNumber, legalName, twoFactorEnabled);
                Users.Add(client);
                AccountManager.CreateAccount(client.UserId, Currency.SEK, 20000, AccountType.CheckingAcc, "My transaction acc.");
                AccountManager.CreateAccount(client.UserId, Currency.SEK, 100000, AccountType.CheckingAcc, "Emergency savings acc.");
                AccountManager.CreateAccount(client.UserId, Currency.SEK, 0, AccountType.LoanAcc, "My Loan acc.");

                return client;
            }
            return null;
        }
        internal static void RemoveUser(User userName)
        {
            Users.Remove(userName);
        }
        internal static User GetUser(string userID)
        {
            foreach (var user in Users)
            {
                if (user.UserId == userID)
                {
                    return user;
                }
            }
            return null;
        }
        internal static bool Authorization(string userID, string inputPassword)
        {
            var user = GetUser(userID);

            if (user.FailedLoginAttempts < 3 && user.CurrentAccountStatus == AccountStatus.Unlocked)
            {
                if (PasswordHasher.VerifyPassword(inputPassword, user.UserPassword))
                {
                    user.UpdateLoginAttempts(0);
                    return true;
                }
                else
                {
                    user.UpdateLoginAttempts(1);
                    if (user.FailedLoginAttempts == 3)
                    {
                        user.UpdateAccountStatus(AccountStatus.Locked);
                    }
                    return false;
                }
            }
            else
            {
                user.UpdateAccountStatus(AccountStatus.Locked);
                return false;
            }
        }

        internal static User Login(string userName, string password)
        {
            foreach (var u in Users)
            {
                if (u.UserId == userName)
                {
                    if (Authorization(u.UserId, password))
                    {
                        if (u.TwoFactorEnabled)
                        {
                            u.TwoFactorCode = GenerateAuthCode(u);
                        }
                        return u;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            return null;
        }

        internal static bool TwoFactorAuth(string userID, string code)
        {
            User user = GetUser(userID);
            if (user.TwoFactorCode == code)
            {
                user.ClearTwoFactorCode();
                return true;
            }
            return false;
        }

        private static string GenerateAuthCode(User u)
        {
            int code = new Random().Next(100000, 1000000);
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Slava Bank", Environment.GetEnvironmentVariable("EMAIL")));
            message.To.Add(new MailboxAddress(u.LegalName, u.Email));
            message.Subject = $"Authentication Code";
            message.Body = new TextPart("plain")
            {
                Text = $@"Continue login with your authentication code: {code}"
            };
            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                client.Authenticate(Environment.GetEnvironmentVariable("EMAIL"), Environment.GetEnvironmentVariable("APP-PASSWORD"));

                client.Send(message);
                client.Disconnect(true);
            }
            return code.ToString();
        }

        internal static void UnlockAccount(User user)
        {
            user.UpdateAccountStatus(AccountStatus.Unlocked);
            user.UpdateLoginAttempts(0);
        }

        public static void ChangeUserInfo(User user, string typeOfChange, string change)
        {
            user.UpdateUserInfo(typeOfChange, change);
        }
    }
}