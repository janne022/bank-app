using bank_app.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bank_app.Utility.UI.Components
{
    public class NavbarItem : UIComponent
    {
        public string Name { get; set; }
        public PageType PageType { get; set; }

        public NavbarItem(string name, PageType page)
        {
            Name = name;
            PageType = page;
        }
        public override (int, int) Pressed()
        {
            PageManager.SwitchPage(PageType);
            return (0, 0);
        }

        public override void Measure()
        {
            Width = Name.Length;
            Height = 1;
        }

        public override void Render()
        {
            Console.SetCursorPosition(X, Y);
            Console.Write(Name);
        }
    }
}
