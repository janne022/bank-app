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
        public bool Pressed()
        {
            bool success = PageManager.SwitchPage(PageType);
            return success;
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
