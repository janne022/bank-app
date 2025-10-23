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
        public Menu(List<UIComponent> components)
        {
            IsInteractable = true;
            _components = components;
        }

        public int ReadOptionIndex(List<UIComponent> menuOptions)
        {
            int originLeft = Console.CursorLeft;
            int originTop = Console.CursorTop;
            int i = 0;
            while (true)
            {
                for (int j = 0; j < menuOptions.Count; j++)
                {
                    int left = Math.Clamp(originLeft, 0, Console.BufferWidth - 1);
                    int top = Math.Clamp(originTop + j, 0, Console.BufferHeight - 1);
                    Console.SetCursorPosition(left, top);
                    menuOptions[j].X = left;
                    menuOptions[j].Y = top;
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
