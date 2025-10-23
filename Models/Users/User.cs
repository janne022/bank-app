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
        private Guid UserId { get; set; }  = Guid.NewGuid();
        public string? UserName { get; set; }
        public string? UserPassword { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        private UserType CurrentUserType {  get; set; }

        protected User( string userName, string userPassword, string email, string phoneNumber, UserType currentUserType)
        {
            UserName = userName;
            UserPassword = userPassword;
            Email = email;
            PhoneNumber = phoneNumber;
            CurrentUserType = currentUserType;
        }
    }
}
