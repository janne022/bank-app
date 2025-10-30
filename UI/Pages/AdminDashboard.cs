using bank_app.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bank_app.UI.Pages
{
    public class AdminDashboard : Page
    {
        public override void LoadPage(User user)
        {
            if (user is Admin admin)
            {
                
            }
        }
    }
}
