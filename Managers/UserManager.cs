using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bank_app.Models.Users;

namespace bank_app.Managers
{
    public enum UserType
    {
        Admin,
        Client
    }

    public static class UserManager
    {
        public static List<User> Users { get; set; } = new List<User>();

        /// <summary>
        /// Method to create new users within the bank app. Adds the newly created user to a list of all users within the application.
        /// </summary>
        /// <param name="userType">Enum that determines whether the user is a client or an administrator</param>

        public static void CreateUser(UserType userType, string userId, string userName, string userPassword)
        {
            if (userType == UserType.Admin)
            {
                var admin = new Admin(userId, userName, userPassword);
                Users.Add(admin);
            }
            else if (userType == UserType.Client)
            {
                var client = new Client(userId, userName, userPassword);
                Users.Add(client);
            }
        }

        public static void RemoveClient()
        {

        }

        public static void ChangeClient()
        {

        }
    }
}
