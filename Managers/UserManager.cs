using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using bank_app.Models.Users;
using bank_app.utility;
using bank_app.Utility;

namespace bank_app.Managers
{
    public static class UserManager
    {
        private static List<User> Users { get; set; } = new List<User>();

        /// <summary>
        /// Method to create new users within the bank app. Adds the newly created user to a list of all users within the application.
        /// </summary>
        /// <param name="userType">Enum that determines whether the user is a client or an administrator</param>
        public static void CreateUser(UserType userType, string userId, string userName, string userPassword, string email = "", string phoneNumber = "")
        {
            if (userType == UserType.Admin)
            {
                var admin = new Admin(userName, userPassword, userType);
                Users.Add(admin);
            }
            else if (userType == UserType.Client)
            {
                var client = new Client(userName, userPassword, email, phoneNumber, userType);
                Users.Add(client);
            }
        }

        /// <summary>
        /// Removes user object from the user list, and nullifies the references of that specific object.
        /// </summary>
        /// <param name="userName">Name of the user object to be removed</param>
        public static void RemoveUser(User userName)
        {
            Users.Remove(userName);
            userName = null;
        }

        /// <summary>
        /// Attempts to authorize a user by verifying the provided password.
        /// </summary>
        public static bool Authorization(User user, string inputPassword)
        {
            if (user.FailedLoginAttempts < 3 && user.CurrentAccountStatus == AccountStatus.Unlocked)
            {
                if (PasswordHasher.VerifyPassword(inputPassword, user.UserPassword))
                {
                    user.FailedLoginAttempts = 0;
                    return true;
                }
                else
                {
                    user.FailedLoginAttempts++;
                    return false;
                }
            }
            else
            {
                user.CurrentAccountStatus = AccountStatus.Locked;
                return false;
                // You have entered the wrong password 3 times, and thusly locked your account. Contact the bank to get your login unlocked.
            }
        }

        public static void UnlockAccount(User user)
        {
            user.CurrentAccountStatus = AccountStatus.Unlocked;
        }

        public static User? Login(string userName, string password)
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

        public static void Logout()
        {
            // UI needed to develop
        }


        /// <summary>
        /// Updates the properties of the user object
        /// </summary>
        /// <param name="user">the name of the user object to be changed</param>
        public static void ChangeUserInfo(User user, string userName, string userPassword)
        {
            user.UserName = userName;
            user.UserPassword = userPassword;
        }
    }
}
