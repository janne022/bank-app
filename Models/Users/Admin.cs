namespace bank_app.Models.Users
{
    public class Admin : User
    {
        public Admin(string userName, string userPassword, string email, string phoneNumber, string legalName, bool twoFactorEnabled = false)
            : base(userName, userPassword, email, phoneNumber, legalName, twoFactorEnabled)
        {
        }
    }
}