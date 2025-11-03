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
        /// <summary>
        /// Method that should be used for loading of the page
        /// </summary>
        /// <param name="user"></param>
        public abstract void LoadPage(User user);
    }
}
