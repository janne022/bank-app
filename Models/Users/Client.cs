using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bank_app.Models.Users
{
    public class Client : User
    {
        public Client(string userName, string userPassword) 
            : base(userName, userPassword)
        {
        }
    }
}
