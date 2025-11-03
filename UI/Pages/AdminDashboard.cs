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
                Console.Clear();
                Console.WriteLine("It's not about how hard you can hit, it's about how har dyou can get hit and KEEP MOVING FORWARD");
            }
        }
    }
}
