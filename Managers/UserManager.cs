using bank_app.Models.Users;
using bank_app.Utility;

namespace bank_app.Managers
{
    public static class UserManager
    {
        private static List<User> Users { get; } = new List<User>();
        public static List<string> UserIds { get; } = new List<string>();

        /// <summary>
        ///  Creates a user object with the inputted arguments
        /// </summary>
        /// <param name="userId">must be a unique login name</param>
        /// <param name="email">must be correctly formatted with @</param>
        /// <param name="phoneNumber">must be correctly formatted with country code and correct amount of digits</param>
        /// <param name="legalName">The individual's legal first name</param>
        /// <returns></returns>
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
                    return false;
                }
            }
            else
            {
                user.UpdateAccountStatus(AccountStatus.Locked);
                return false;
                // You have entered the wrong password 3 times, and thusly locked your account. Contact the bank to get your login unlocked.
            }
        }

        internal static void UnlockAccount(User user)
        {
            user.UpdateAccountStatus(AccountStatus.Unlocked);
        }

        internal static User? Login(string userName, string password)
        {
            List<User> sameNameList = new List<User>();

            // Loops through all made users in the program...
            foreach (var u in Users)
            {
                // if more than one user in the app has the same name i.e. several Eriks...
                if (u.UserId == userName)
                {
                    //...add them to a local list.
                    sameNameList.Add(u);
                }
            }

            //... if there is only one person with that name...
            if (sameNameList.Count == 1)
            {
                // try and authorize. If it also is the correct password...
                if (Authorization(sameNameList[0].UserId.ToString(), password))
                {
                    // ...return that specific user
                    return sameNameList[0];
                }
                else
                {
                    sameNameList[0].UpdateLoginAttempts(1);
                    return null;
                }
            }
            // if there are several people with the same name i.e. several Eriks...
            else if (sameNameList.Count > 1)
            {
                //... loop through these people and...
                foreach (var sameNameUser in sameNameList)
                {
                    //...if they match with the correct password...
                    if (Authorization(sameNameUser.UserId.ToString(), password))
                    {
                        // return
                        return sameNameUser;
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
