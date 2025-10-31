using bank_app.Models.Users;
using bank_app.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bank_app.UI.Pages
{
    public class ClientDashboard : Page
    {
        public override void LoadPage(User user)
        {
            if (user is Client client)
            {
                Console.WriteLine("Välkommen till dashboard: " + client.UserName);
                Console.ReadLine();
            }
        }
    }
}
