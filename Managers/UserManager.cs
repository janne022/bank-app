using bank_app.Models.Users;
using bank_app.Utility;

namespace bank_app.Managers
{
    public static class UserManager
    {
        private static List<User> Users { get; } = new List<User>();

        /// <summary>
        /// Method to create new users within the bank app. Adds the newly created user to a list of all users within the application.
        /// </summary>
        /// <param name="userType">Enum that determines whether the user is a client or an administrator</param>

        internal static void CreateUser(string userName, string userPassword, UserType userType, string email = "", string phoneNumber = "")
        {
            if (userType is UserType.Admin)
            {
                var admin = new Admin(userName, userPassword);
                Users.Add(admin);
            }
            else if (userType is UserType.Client)
            {
                var client = new Client(userName, userPassword, email, phoneNumber);
                Users.Add(client);
            }

        }

        /// <summary>
        /// Removes user object from the user list, and nullifies the references of that specific object.
        /// </summary>
        /// <param name="userName">Name of the user object to be removed</param>
        internal static void RemoveUser(User userName)
        {
            Users.Remove(userName);
        }

        /// <summary>
        /// Attempts to authorize a user by verifying the provided password.
        /// </summary>
        internal static bool Authorization(User user, string inputPassword)
        {
            if (user.FailedLoginAttempts < 3 && user.CurrentAccountStatus == AccountStatus.Unlocked)
            {
                if (PasswordHasher.VerifyPassword(inputPassword, user.UserPassword))
                {
                    user.updateLoginAttempts(0);
                    return true;
                }
                else
                {
                    user.updateLoginAttempts(1);
                    return false;
                }
            }
            else
            {
                user.updateAccountStatus(AccountStatus.Locked);
                return false;
                // You have entered the wrong password 3 times, and thusly locked your account. Contact the bank to get your login unlocked.
            }
        }

        internal static void UnlockAccount(User user)
        {
            user.updateAccountStatus(AccountStatus.Unlocked);
        }

        internal static User? Login(string userName, string password)
        {
            // Loops through all made users in the program...
            foreach (var user in Users)
            {
                // if the username input matches any of the existing users' names... AND they are authorized to login AND the password is correct...
                if (user.UserName == userName && Authorization(user, password))
                {
                    // ...return that specific user
                    return user;
                }
            }
            // ...and if no user matches the name, password or the user is not authorized to login: return nothing.
            return null;
        }

        internal static void Logout()
        {
            // UI needed to develop
        }


        /// <summary>
        /// Updates the properties of the user object
        /// </summary>
        /// <param name="user">the name of the user object to be changed</param>
        public static void ChangeUserInfo(User user, string typeOfChange, string change)
        {
            user.updateUserInfo(typeOfChange, change);
        }
    }
}
