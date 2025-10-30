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
    public class PageManager
    {
        private readonly Dictionary<PageType, Page> _pageDictionary = new()
        {
            {PageType.Login,  new Login()},
            {PageType.ClientDashboard,  new ClientDashboard()},
            {PageType.AdminDashboard,  new AdminDashboard()}
        };
        public void SwitchPage(PageType page, User user)
        {
            Console.Clear();
            _pageDictionary[page].LoadPage(user);
        }

        public void Start(PageType firstPage, User user)
        {
            _pageDictionary[firstPage].OnSwitchPageRequest += SwitchPage;
            SwitchPage(firstPage, user);
        }
    }
}
