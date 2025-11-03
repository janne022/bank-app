using bank_app.Models.Users;
using bank_app.Utility;

namespace bank_app.Managers
{
    public static class UserManager
    {
        private static List<User> Users { get; } = new List<User>();
        public static List<string> UserIds { get; } = new List<string>();

        /// <summary>
        /// Method to create new users within the bank app. Adds the newly created user to a list of all users within the application.
        /// </summary>
        /// <param name="userType">Enum that determines whether the user is a client or an administrator</param>

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

        /// <summary>
        /// Removes user object from the user list, and nullifies the references of that specific object.
        /// </summary>
        /// <param name="userName">Name of the user object to be removed</param>
        internal static void RemoveUser(User userName)
        {
            Users.Remove(userName);
        }

        internal static User GetUser(string userID)
        {
            foreach (var user in Users)
            {
                if (user.UserId.ToString() == userID)
                {
                    return user;
                }
            }

            return null;
        }

        /// <summary>
        /// Attempts to authorize a user by verifying the provided password.
        /// </summary>
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

                //... if there is only one person with that name...
                if (sameNameList.Count == 1)
                {
                    // try and authorize. If it also is the correct password...
                    if (Authorization(u.UserId.ToString(), password))
                    {
                        // ...return that specific user
                        return u;
                    }
                    else
                    {
                        u.UpdateLoginAttempts(1);
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
                else
                {
                    return null;
                }

            }

            // ...and if no user matches the name, password or the user is not authorized to login: return nothing.
            return null;
        }


        ////New Method to Login user with User Id
        //internal static User? Login(string userId, string password)
        //{

        //    var user = Users.FirstOrDefault(u => u.UserId.ToString() == userId && Authorization(u.UserId.ToString(), password));

        //    if (user == null)
        //    {
        //        return null;
        //    }

        //    return user;
        //}
        internal static void Logout()
        {
            // UI needed to develop - change page to login page
        }


        /// <summary>
        /// Updates the properties of the user object
        /// </summary>
        /// <param name="user">the name of the user object to be changed</param>
        public static void ChangeUserInfo(User user, string typeOfChange, string change)
        {
            user.UpdateUserInfo(typeOfChange, change);
        }
    }
}
