using bank_app.Managers;
using bank_app.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bank_app.Utility
{
    internal static class SeedData
    {
        public static void CreateSeedData()
        {
            UserManager.CreateUser("admin", "admin", UserType.Admin, "admin@gmail.com", "0709441133", "Creator");
            UserManager.CreateUser("aldor", "ivarandkidaandleia4ever", UserType.Admin, "aldor@aldor.se", "070 928 345", "Great One");

            User vira = UserManager.CreateUser("glorfindel", "glorfindel", UserType.Client, "elvira.test@chasacademy.se", "070729292", "Elvira");
            User emma = UserManager.CreateUser("ishtar", "ishtar", UserType.Client, "emma.test@chasacademy.se", "070729293", "Emma");
            User janne = UserManager.CreateUser("sourdough_king", "sourdough", UserType.Client, "janne.test@chasacademy.se", "070729294", "Johannes");
            User slava = UserManager.CreateUser("gif_lord", "giflord", UserType.Client, "slava.test@chasacademy.se", "070729295", "Slava");
            User iskld = UserManager.CreateUser("skitfiske", "skitfiske", UserType.Client, "theo.test@chasacademy.se", "070729296", "Theo");


            // gives each user three accounts; two checking accounts and one saving account
            foreach (var user in UserManager.Users)
            {
                AccountManager.CreateAccount(user.UserId, Currency.SEK, 100000m, AccountType.CheckingAcc);
                AccountManager.CreateAccount(user.UserId, Currency.SLC, 0.00005m, AccountType.CheckingAcc);
                AccountManager.CreateAccount(user.UserId, Currency.SEK, 60000m, AccountType.SavingsAcc);
            }

            // -------------------------------------- some transactions ------------------------ //
            TransactionManager.CreateNewTransaction(
                AccountManager.GetAllAccounts(vira.UserId)[0].AccountID,
                AccountManager.GetAllAccounts(slava.UserId)[0].AccountID,
                20000m
                );

            TransactionManager.CreateNewTransaction(
                 AccountManager.GetAllAccounts(janne.UserId)[2].AccountID,
                 AccountManager.GetAllAccounts(emma.UserId)[2].AccountID,
                 0.0000001m
                 );

            TransactionManager.CreateNewTransaction(
                 AccountManager.GetAllAccounts(iskld.UserId)[0].AccountID,
                 AccountManager.GetAllAccounts(emma.UserId)[0].AccountID,
                 2000m
                 );

            TransactionManager.CreateNewTransaction(
                 AccountManager.GetAllAccounts(janne.UserId)[0].AccountID,
                 AccountManager.GetAllAccounts(emma.UserId)[0].AccountID,
                 5000m
                 );
            TransactionManager.ProcessPendingTransactions();
        }
    }
}
