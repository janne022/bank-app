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
        public List<Account> MyAccounts { get; } = new List<Account>();

        public Client(string userName, string userPassword, string email, string phoneNumber, string legalName)
            : base(userName, userPassword, email, phoneNumber, legalName)
        {
            Email = email;
            PhoneNumber = phoneNumber;
        }


        // private setters
        private void SetEmail(string email)
        {
            Email = email;
        }

        private void SetPhoneNumber(string phoneNumber)
        {
            PhoneNumber = phoneNumber;
        }

        // public methods to reach private setters

        public void UpdateContactInfo(string type, string change)
        {
            switch (type)
            {
                case "email":
                    SetEmail(change);
                    break;

                case "number":
                    SetPhoneNumber(change);
                    break;
            }
        }
    }

}
