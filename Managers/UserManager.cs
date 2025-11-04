using bank_app.Models.Users;
using bank_app.Utility;

namespace bank_app.Managers
{
    public static class UserManager
    {
        private static List<User> Users { get; } = new List<User>();
        public static List<string> UserIds { get; } = new List<string>();

        internal static User CreateUser(string userId, string userPassword, UserType userType, string email, string phoneNumber, string legalName)
        {
            if (userType is UserType.Admin)
            {
                var admin = new Admin(userId, userPassword, email, phoneNumber, legalName);
                Users.Add(admin);
                return admin;
            }
            else if (userType is UserType.Client)
            {
                var client = new Client(userId, userPassword, email, phoneNumber, legalName);
                Users.Add(client);
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

        internal static void UnlockAccount(User user)
        {
            user.UpdateAccountStatus(AccountStatus.Unlocked);
        }

        internal static User Login(string userName, string password)
        {
            foreach (var u in Users)
            {
                if (u.UserId == userName)
                {
                    if (Authorization(u.UserId, password))
                    {
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

        internal static void Logout()
        {
            // UI needed to develop - change page to login page
        }

        public static void ChangeUserInfo(User user, string typeOfChange, string change)
        {
            user.UpdateUserInfo(typeOfChange, change);
        }
    }
}