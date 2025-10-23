using bank_app.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bank_app.Models.Users
{
    public class Admin : User
    {
        public Admin(string userName, string userPassword, string email, string phoneNumber, UserType currentUserType) 
            : base( userName, userPassword, email, phoneNumber, currentUserType)
        {
            UserName = "admin";
            UserPassword = "chasbanken";
        }
    }
}
