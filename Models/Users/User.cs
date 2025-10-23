using bank_app.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bank_app.Models.Users
{
    public abstract class User
    {
        public Guid UserId { get; set; }  = Guid.NewGuid();
        public string? UserName { get; set; }
        public string? UserPassword { get; set; }

        private UserType userType {  get; set; }

        protected User( string userName, string userPassword)
        {
            UserName = userName;
            UserPassword = userPassword;
        }
    }
}
