using bank_app.Models.Accounts;
using bank_app.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bank_app.Models.Users
{
    public class Client : User
    {
        internal string? Email { get; private set; }
        internal string? PhoneNumber { get; private set; }
        internal List<Account> MyAccounts { get; private set; } = new List<Account>();

        public Client(string userName, string userPassword, string email, string phoneNumber)
            : base(userName, userPassword)
        {
            Email = email;
            PhoneNumber = phoneNumber;
        }
    }
}
