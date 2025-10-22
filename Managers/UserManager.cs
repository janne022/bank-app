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
