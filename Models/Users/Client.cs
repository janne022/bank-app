using bank_app.Models.Accounts;

namespace bank_app.Models.Users
{
    public class Client : User
    {
        public List<Account> MyAccounts { get; } = new List<Account>();
        public Client(string userName, string userPassword, string email, string phoneNumber, string legalName, bool twoFactorEnabled = false)
            : base(userName, userPassword, email, phoneNumber, legalName, twoFactorEnabled)
        {
        }
    }
}