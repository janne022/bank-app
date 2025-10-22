using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bank_app.Models.Users
{
    public abstract class User
    {
        public string? UserId { get; set; }  
        public string? UserName { get; set; }
        public string? UserPassword { get; set; }

        protected User(string userId, string userName, string userPassword)
        {
            UserId = userId;
            UserName = userName;
            UserPassword = userPassword;
        }
    }
}
