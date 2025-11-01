using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace bank_app.Utility.UI.Components
{
    public class Navbar : UIComponent
    {
        public List<NavbarItem> Items { get; set; }
        private int _index = 0;
        public Navbar(List<NavbarItem> items)
        {
            IsInteractable = true;
            Items = items;
        }

        public void AddNavbarItem(NavbarItem item)
        {
            Items.Add(item);
        }

        public override (int, int) Pressed()
        {
            // Holds a menu in a while loop
            while (true)
            {
                // Render every object after eachother. If selected object is the one we are rendering, we highlight it
                for (int j = 0; j < Items.Count; j++)
                {
                    if (j == _index)
                    {
                        // Black foreground on white background
                        Console.Write($"\u001b[30;47m");
                        Items[j].Render();
                        Console.Write("\u001b[0m");
                    }
                    else
                    {
                        Items[j].Render();
                    }
                }
                // Read key and if user presses up or down we add or subtract from i. If user presses Enter we run the components pressed method.
                ConsoleKey key = Console.ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.RightArrow:
                        if (_index < Items.Count - 1)
                        {
                            _index++;
                        }
                        else
                        {
                            return (0, 1);
                        }
                        break;
                    case ConsoleKey.LeftArrow:
                        if (_index > 0)
                        {
                            _index--;
                        }
                        else
                        {
                            return (0, -1);
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        return (1, 0);
                    case ConsoleKey.UpArrow:
                        return (-1, 0);
                    case ConsoleKey.Enter:
                        Items[_index].Pressed();
                        return (0, 0);
                }
            }
        }

        public override void Render()
        {
            for (int j = 0; j < Items.Count; j++)
            {
                Items[j].Measure();
                Items[j].X = X + (j * Items[j].Width);
                Items[j].Y = Y;
                Items[j].Render();
            }
        }
    }
}
