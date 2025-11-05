using bank_app.Models.Users;
using bank_app.Utility;
using bank_app.Utility.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bank_app.UI
{
    public abstract class Page
    {
        internal User? CurrentUser { get; set; }
        internal bool ContinueRunning { get; set; }

        /// <summary>
        /// Method that should be used for loading of the page
        /// </summary>
        /// <param name="user"></param>
        internal abstract UIComponent LoadPage();
        public void DisplayPage(User user)
        {
            Console.CursorVisible = false;
            CurrentUser = user;
            var rootElement = LoadPage();
            rootElement.Render();
            while (ContinueRunning)
            {
                rootElement.Pressed();
            }
        }
    }
}
