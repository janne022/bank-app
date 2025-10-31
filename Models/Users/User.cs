using bank_app.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bank_app.Utility;

namespace bank_app.Models.Users
{
    public abstract class User
    {
        private Guid UserId { get; }  = Guid.NewGuid();
        public string? UserName { get; set; }

        private string? UserPassword { get; set; };
        public int FailedLoginAttempts { get; set; } = 0;
        public AccountStatus CurrentAccountStatus { get; set; }

        protected User( string userName, string userPassword)
        {
            UserName = userName;
            _UserPassword = PasswordHasher.Hash(userPassword);
        }
    }
}
