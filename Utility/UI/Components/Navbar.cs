using bank_app.UI;

namespace bank_app.Utility.UI.Components
{
    public class Navbar : UIComponent
    {
        public List<NavbarItem> Items { get; set; }
        private int _index = 0;
        private const string Separator = " · ";

        /// <summary>
        /// Navbar that takes a list of navbar items and whenever one of those are clicked it will navigate to that page
        /// </summary>
        /// <param name="navItems">the list of navbar items</param>
        public Navbar(List<NavbarItem> navItems, int marginTop = 0, int marginLeft = 0, int marginRight = 0, int marginBottom = 0) : base(marginTop, marginLeft, marginRight, marginBottom)
        {
            IsMultiComponent = true;
            Items = navItems;
            _index = Items.FindIndex(item => item.PageType == PageManager.GetCurrentPageType());
            if (_index == -1)
            {
                _index = 0;
            }
        }

        public void AddNavbarItem(NavbarItem item)
        {
            Items.Add(item);
        }

        public override void Measure()
        {
            foreach (var item in Items)
            {
                item.Measure();
            }
            Height = 1 + MarginBottom + MarginTop;
            // Measure width with all nav items and with seperator
            var sepCount = Math.Max(0, Items.Count - 1);
            Width = Items.Sum(navItem => navItem.Width) + (sepCount * Separator.Length) + MarginLeft + MarginRight;
        }

        public override (int, int) Pressed()
        {
            // Holds a menu in a while loop
            while (true)
            {
                // Layout and render at absolute positions to avoid overwriting separators
                int totalWidth = 0;
                for (int j = 0; j < Items.Count; j++)
                {
                    Items[j].Measure();
                    Items[j].X = X + totalWidth;
                    Items[j].Y = Y;

                    if (j == _index)
                    {
                        // Black foreground on white background
                        Console.Write("\u001b[30;47m");
                        Items[j].Render();
                        Console.Write("\u001b[0m");
                    }
                    else
                    {
                        Items[j].Render();
                    }

                    totalWidth += Items[j].Width;

                    if (j != Items.Count - 1)
                    {
                        // Place separator
                        Console.SetCursorPosition(X + totalWidth, Y);
                        Console.Write(Separator);
                        totalWidth += Separator.Length;
                    }
                }

                // Read key and if user presses left/right/up/down, update index. Enter runs Pressed that navigates to the website
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
                        bool success = Items[_index].Pressed();
                        if (success)
                        {
                            return (0, 0);
                        }
                        else
                        {
                            break;
                        }
                }
            }
        }

        public override void Render()
        {
            int totalWidth = 0;
            for (int j = 0; j < Items.Count; j++)
            {
                Items[j].Measure();
                Items[j].X = X + totalWidth;
                Items[j].Y = Y;
                Items[j].Render();

                totalWidth += Items[j].Width;

                if (j != Items.Count - 1)
                {
                    Console.SetCursorPosition(X + totalWidth, Y);
                    Console.Write(Separator);
                    totalWidth += Separator.Length;
                }
            }
        }
    }
}
