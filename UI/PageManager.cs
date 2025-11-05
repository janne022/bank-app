using bank_app.Models.Users;
using bank_app.UI.Pages;
using bank_app.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bank_app.UI
{
    public static class PageManager
    {
        // Dictionary containing all pages that exist
        private static readonly Dictionary<PageType, Page> _pageDictionary = new()
        {
            {PageType.Login,  new Login()},
            {PageType.ClientDashboard,  new ClientDashboard()},
            {PageType.Transfer,  new Transfer()},
            {PageType.Transaction,  new TransactionPage()},
            {PageType.Account,  new Account()},
            {PageType.Loan,  new LoanPage()},
            {PageType.AdminDashboard,  new AdminDashboard()},
            {PageType.CreateUser, new CreateUser()},
            {PageType.CheckTransactions, new CheckTransactions()},
            {PageType.UpdateRates, new UpdateRates()}
        };
        private static bool _isRunning = true;
        private static Page? _nextPage;
        private static Page? _currentPage;
        private static User? _currentUser;
        private static PageType _currentPageType;

        /// <summary>
        /// Switches to specified page
        /// </summary>
        /// <param name="page">Chosen page to switch to</param>
        public static bool SwitchPage(PageType page)
        {
            if (page != _currentPageType)
            {
                _nextPage = _pageDictionary[page];
                _currentPageType = page;
                if (_currentPage != null)
                {
                    _currentPage.ContinueRunning = false;
                }
                return true;
            }
            return false;
        }

        public static PageType GetCurrentPageType()
        {
            return _currentPageType;
        }

        /// <summary>
        /// Switches the user forwarded to each page
        /// </summary>
        /// <param name="user">Specified user to forward</param>
        public static void SwitchUser(User user)
        {
            _currentUser = user;
        }

        public static User? GetCurrentUser()
        {
            return _currentUser;
        }

        /// <summary>
        /// Stops the application loop
        /// </summary>
        public static void Stop()
        {
            _isRunning = false;
        }

        /// <summary>
        /// Starts the application loop, initializing the user and loading the specified start page.
        /// </summary>
        /// <remarks>The method runs a continuous loop, monitoring and loading pages as needed. The
        /// application  will remain in this loop until PageManager.Stop() runs</remarks>
        /// <param name="startPage">The initial page to load when the application starts.</param>
        public static void Start(PageType startPage)
        {
            _nextPage = _pageDictionary[startPage];
            _currentPageType = startPage;
            while (_isRunning)
            {
                if (_currentPage != _nextPage)
                {
                    Console.Clear();
                    _currentPage = _nextPage;
                    _currentPage.ContinueRunning = true;
                    _currentPage?.DisplayPage(_currentUser);
                }
                Thread.Sleep(100);
            }
        }
    }
}
