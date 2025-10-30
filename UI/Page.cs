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
        public event Action<PageType, User>? OnSwitchPageRequest;

        protected void RequestPageChange(PageType page, User user)
        {
            OnSwitchPageRequest?.Invoke(page, user);
        }
        public abstract void LoadPage(User user);
    }
}
