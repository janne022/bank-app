using bank_app.Models.Users;
using bank_app.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bank_app.UI
{
    public abstract class Page
    {
        // Event that should contain the SwitchPage method via PageManager
        public event Action<PageType, User>? OnSwitchPageRequest;

        /// <summary>
        /// Use this method to switch to a certain page from the current page
        /// </summary>
        /// <param name="page">The specific page you want to switch to</param>
        /// <param name="user">The user object. For pages that don't need a user you can fill out an empty object</param>
        protected void RequestPageChange(PageType page, User user)
        {
            OnSwitchPageRequest?.Invoke(page, user);
        }
        /// <summary>
        /// Method that should be used for loading of the page
        /// </summary>
        /// <param name="user"></param>
        public abstract void LoadPage(User user);
    }
}
