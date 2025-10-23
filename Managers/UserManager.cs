using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bank_app.Models.Users;
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
        /// Attempts to authorize a user based on the provided credentials.
        /// </summary>
        /// <remarks>The method allows up to three attempts to validate the password. If all attempts
        /// fail, the account status is set to <see cref="AccountStatus.Locked"/>.</remarks>
        /// <returns>An <see cref="AccountStatus"/> value indicating the result of the authorization attempt. Returns <see
        /// cref="AccountStatus.Open"/> if the password matches the user's stored password; otherwise, returns <see
        /// cref="AccountStatus.Locked"/> after three failed attempts.</returns>
        public static AccountStatus AuthorizeUser(User user, string password)
        {
            int timesTried = 0;

            while (timesTried != 3)
            {
                if (user.UserPassword == password)
                {
                    return AccountStatus.Open;
                }
                timesTried++;
            }

            return AccountStatus.Locked;
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
