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
            {PageType.AdminDashboard,  new AdminDashboard()}
        };

        // Used for switching to a specific page
        private static void SwitchPage(PageType page, User user)
        {
            Console.Clear();
            var selectedPage = _pageDictionary[page];
            selectedPage.OnSwitchPageRequest += OnSwitchPage;
            selectedPage.LoadPage(user);
        }
        private static void OnSwitchPage(PageType firstPage, User user)
        {
            SwitchPage(firstPage, user);
        }
        // Initial start to load the first page and subscribe SwitchPage to first pages event
        public static void Start(PageType firstPage, User user)
        {
            SwitchPage(firstPage, user);
        }
    }
}
