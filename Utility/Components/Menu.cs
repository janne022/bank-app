using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bank_app.Utility.Components
{
    public class Menu : UIComponent
    {
        private List<UIComponent> _components;
        private object[] args;
        public Menu(List<UIComponent> components, int width = 20)
        {
            IsInteractable = true;
            _components = components;
            Width = width;
            foreach (var item in components)
            {
                item.ParentElement = this;
            }
        }

        public int ReadOptionIndex(List<UIComponent> menuOptions)
        {
            int argsCount = 0;
            for (int j = 0; j < menuOptions.Count; j++)
            {
                if (menuOptions[j] is not Button)
                {
                    argsCount++;
                }
            }
            args = new object[argsCount];
            int i = 0;
            while (true)
            {
                for (int j = 0; j < menuOptions.Count; j++)
                {
                    menuOptions[j].X = X;
                    menuOptions[j].Y = Y + j;
                    if (j == i)
                    {
                        // Black foreground on white background
                        Console.Write($"\u001b[30;47m");
                        menuOptions[j].Render();
                        Console.Write("\u001b[0m");
                    }
                    else
                    {
                        menuOptions[j].Render();
                    }
                }
                ConsoleKey key = Console.ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.DownArrow:
                        if (i < menuOptions.Count - 1)
                        {
                            i++;
                        }
                        break;
                    case ConsoleKey.UpArrow:
                        if (i > 0)
                        {
                            i--;
                        }
                        break;
                    case ConsoleKey.Enter:
                        menuOptions[i].Pressed();
                        if (menuOptions[i] is not Button)
                        {
                            args[i] = menuOptions[i].Value;
                        }
                        else if (menuOptions[i] is Button button)
                        {
                            button.Pressed(args);
                        }
                        break;

                }
            }
        }

        public override void Render()
        {
            ReadOptionIndex(_components);
        }
    }
}
