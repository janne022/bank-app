using bank_app.Managers;
using bank_app.Models.Users;

namespace bank_app.Utility
{
    internal static class SeedData
    {
        public static void CreateSeedData()
        {
            // -------------------------------------- seed data of us creators of the app + an admin account ------ //

            User admin = UserManager.CreateUser("admin", "admin", UserType.Admin, "admin@gmail.com", "0709441133", "Creator");
            User vira = UserManager.CreateUser("glorfindel", "glorfindel", UserType.Client, "elvira.mariesdotter+slavabank@chasacademy.se", "070729292", "Elvira M", true);
            User emma = UserManager.CreateUser("ishtar", "ishtar", UserType.Client, "emma.test@chasacademy.se", "070729293", "Emma K");
            User janne = UserManager.CreateUser("janne", "sourdough", UserType.Client, "johannes.flodin+slavabank@chasacademy.se", "070729294", "Johannes F", true);
            User slava = UserManager.CreateUser("gif_lord", "giflord", UserType.Client, "slava.test@chasacademy.se", "070729295", "Slava L");
            User iskld = UserManager.CreateUser("skitfiske", "skitfiske", UserType.Client, "theo.test@chasacademy.se", "070729296", "Theo L");



            // gives each user three accounts; two checking accounts and one saving account
            foreach (var user in UserManager.Users)
            {
                AccountManager.CreateAccount(user.UserId, Currency.SLC, 0.5m, AccountType.CheckingAcc, "Slava coins");
            }



            // --------------------------- Some fake companies ---------------------------------------- //

            User aldorLivs = UserManager.CreateUser("aldorLivs", "aldorLivs", UserType.Client, "livsAB@aldorlivs.se", "080707071", "Aldor Livs AB");
            User pelleblues = UserManager.CreateUser("pelleblues", "pelleblues", UserType.Client, "livsAB@aldorlivs.se", "080707072", "Pelle blues HB");
            User theocarz = UserManager.CreateUser("theocar", "theocar", UserType.Client, "livsAB@aldorlivs.se", "080707073", "Theo Carz shop");
            User jannebakery = UserManager.CreateUser("jannebread", "jannebread", UserType.Client, "livsAB@aldorlivs.se", "080707073", "J. bakery AB");
            User slavagym = UserManager.CreateUser("slavagym", "slavagym", UserType.Client, "livsAB@aldorlivs.se", "080707073", "Slava fit 25/7");
            User emmamanga = UserManager.CreateUser("emmamanga", "emmamanga", UserType.Client, "livsAB@aldorlivs.se", "080707073", "Em's mangstore");
            User virabook = UserManager.CreateUser("virabook", "virabook", UserType.Client, "livsAB@aldorlivs.se", "080707073", "V book shop");


            // --------------------------- some transactions ------------------------ //

            TransactionManager.CreateNewTransaction(
                AccountManager.GetAllAccounts(vira.UserId)[0].AccountID,
                AccountManager.GetAllAccounts(slava.UserId)[0].AccountID,
                2000m
                );

            TransactionManager.CreateNewTransaction(
                 AccountManager.GetAllAccounts(janne.UserId)[3].AccountID,
                 AccountManager.GetAllAccounts(emma.UserId)[3].AccountID,
                 0.0000001m
                 );

            TransactionManager.CreateNewTransaction(
                 AccountManager.GetAllAccounts(iskld.UserId)[0].AccountID,
                 AccountManager.GetAllAccounts(emma.UserId)[0].AccountID,
                 2000m
                 );

            TransactionManager.CreateNewTransaction(
                 AccountManager.GetAllAccounts(emma.UserId)[0].AccountID,
                 AccountManager.GetAllAccounts(janne.UserId)[0].AccountID,
                 5000m
                 );

            // -------------------------------------- RANDOMIZED MASS TRANSACTIONS -------------------------------------- //

            var users = new[] { vira, emma, janne, slava, iskld, aldorLivs, pelleblues, slavagym, emmamanga, jannebakery, theocarz, virabook };
            var rand = new Random();

            for (int i = 0; i < 420; i++)
            {
                var sender = users[rand.Next(users.Length)];
                var receiver = users[rand.Next(users.Length)];

                if (sender == receiver)
                    continue;

                var senderAcc = AccountManager.GetAllAccounts(sender.UserId)[rand.Next(2)];
                var receiverAcc = AccountManager.GetAllAccounts(receiver.UserId)[rand.Next(2)];

                decimal amount = RandomDecimal(50, 5000);

                TransactionManager.CreateNewTransaction(
                    senderAcc.AccountID,
                    receiverAcc.AccountID,
                    amount
                );
            }

            TransactionManager.ProcessPendingTransactions();
        }

        private static decimal RandomDecimal(decimal min, decimal max)
        {
            var rand = new Random(Guid.NewGuid().GetHashCode());
            double range = (double)(max - min);
            double sample = rand.NextDouble();
            decimal value = (decimal)(sample * range) + min;
            return Math.Round(value, 2);
        }
    }
}