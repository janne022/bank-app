using bank_app.Utility;
using bank_app.Utility.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bank_app.UI
{
    public static class Login
    {
        public static void Menu()
        {
            List<UIComponent> components = new List<UIComponent>
            {
                new Text("Login"),
                new InputField("Username",false,16),
                new InputField("Password",true,16),
                new Button(),
            };
            Grid grid = new Grid(components,10,10, Justify.Center,Align.Middle, OrderBy.Column);
            Layout layout = new Layout(grid, LayoutBorder.Rounded);
            layout.Render();
        }
    }
}
