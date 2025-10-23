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
        public Client(string userName, string userPassword, string email, string phoneNumber, UserType currentUserType) 
            : base(userName, userPassword, email, phoneNumber, currentUserType)
        {
        }
    }
}
