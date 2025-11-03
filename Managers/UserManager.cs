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

        internal static User CreateUser(string userName, string userPassword, UserType userType, string email = "", string phoneNumber = "")
        {
            if (userType is UserType.Admin)
            {
                var admin = new Admin(userName, userPassword);
                Users.Add(admin);
                return admin;
            }
            else if (userType is UserType.Client)
            {
                var client = new Client(userName, userPassword, email, phoneNumber);
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
                if(user.UserId.ToString() == userID)
                {
                    return user;
                }
            }

            return null;
        }

        /// <summary>
        /// Attempts to authorize a user by verifying the provided password.
        /// </summary>
        /// 
        //Elvira kolla om du vill ändra den metoden
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
      
        //This method can be deleted if you want 
        //internal static User? Login(User user, string password)
        //{
        //    // Loops through all made users in the program...
        //    foreach (var u in Users)
        //    {
        //        // if the user id input matches any of the existing users' ids... AND they are authorized to login AND the password is correct...
        //        if (u.UserId == user.UserId && Authorization(user, password))
        //        {
        //            // ...return that specific user
        //            return u;
        //        }
        //    }
        //    // ...and if no user matches the name, password or the user is not authorized to login: return nothing.
        //    return null;
        //}


        //New Method to Login user with User Id
        internal static User? Login(string userId, string password)
        {
            
           var user= Users.FirstOrDefault(u => u.UserId.ToString() == userId && Authorization(u.UserId.ToString(), password));

            if (user==null)
            {
                return null;
            }

            return user;
        }
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
