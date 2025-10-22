using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bank_app.Models.Users
{
    public class Admin : User
    {
        public Admin(string userId, string userName, string userPassword) 
            : base(userId, userName, userPassword)
        {
            UserId = "ADM2947XZ";
            UserName = "admin";
            UserPassword = "chasbanken";
        }
    }
}
