using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bank_app.Utility
{

    public enum UserType
    {
        Admin,
        Client
    }

    public enum TransactionStatus
    {
        Pending,
        Completed,
        Failed,
    }
}
