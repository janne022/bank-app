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
        public string? Email { get; private set; }
        public string? PhoneNumber { get; private set; }
        public List<Account> MyAccounts { get; set; } = new List<Account>();

        public Client(string userName, string userPassword, string email, string phoneNumber) 
            : base(userName, userPassword)
        {
            Email = email;
            PhoneNumber = phoneNumber;
        }


        // private setters
        private void setEmail(string email)
        {
            Email = email;
        }

        private void setPhoneNumber(string phoneNumber)
        {
            PhoneNumber = phoneNumber;
        }

        // public methods to reach private setters

        public void updateContactInfo(string type, string change)
        {
            switch (type)
            {
                case "email":
                    setEmail(change);
                    break;

                case "number":
                    setPhoneNumber(change);
                    break;
            }
        }
    }

}
