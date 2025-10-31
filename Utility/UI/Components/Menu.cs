using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bank_app.Utility.UI.Components
{
    public class Menu : UIComponent
    {
        private List<UIComponent> _components;
        private object[] args;
        /// <summary>
        /// Initializes a new instance of the <see cref="Menu"/> that renders and handles
        /// keyboard navigation for a list of child <see cref="UIComponent"/>s.
        /// </summary>
        /// <param name="components">
        /// The child components to display inside the menu, in render/navigation order.
        /// The menu sets itself as each child's <see cref="UIComponent.ParentComponent"/>.
        /// </param>
        /// <param name="order">
        /// Intended layout ordering for the menu items (row or column). Rendering currently
        /// behaves as a column list; this parameter is reserved for future layout behavior.
        /// </param>
        /// <remarks>
        /// The menu is interactive by default. It highlights the currently selected item and
        /// supports Up/Down arrow navigation and Enter to activate the selected component.
        /// When a non-<see cref="Button"/> component is activated, its <see cref="UIComponent.Value"/>
        /// is captured internally. When a <see cref="Button"/> is activated, all captured values
        /// are passed to the button via <c>Pressed(object[] args)</c>.
        /// </remarks>
        public Menu(List<UIComponent> components, OrderBy order = OrderBy.Column, int width = 30)
        {
            // Set variables
            IsInteractable = true;
            _components = components;
            Width = width;
            Height = components.Count;

            // Make this object a parent to all child objects
            foreach (var item in components)
            {
                item.ParentComponent = this;
            }
            // Check how many objects that are not buttons to see how many arguments we are passing if there is a button inside Menu
            int argsCount = 0;
            for (int j = 0; j < components.Count; j++)
            {
                if (components[j] is not Button)
                {
                    argsCount++;
                }
            }
            args = new object[argsCount];
        }

        public override (int, int) Pressed()
        {
            // Holds a menu in a while loop
            int i = 0;
            while (true)
            {
                // Render every object after eachother. If selected object is the one we are rendering, we highlight it
                for (int j = 0; j < _components.Count; j++)
                {
                    _components[j].X = X;
                    _components[j].Y = Y + j;
                    if (j == i)
                    {
                        // Black foreground on white background
                        Console.Write($"\u001b[30;47m");
                        _components[j].Render();
                        Console.Write("\u001b[0m");
                    }
                    else
                    {
                        _components[j].Render();
                    }
                }
                // Read key and if user presses up or down we add or subtract from i. If user presses Enter we run the components pressed method.
                ConsoleKey key = Console.ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.DownArrow:
                        if (i < _components.Count - 1)
                        {
                            i++;
                        }
                        else
                        {
                            return (1, 0);
                        }
                        break;
                    case ConsoleKey.UpArrow:
                        if (i > 0)
                        {
                            i--;
                        }
                        else
                        {
                            return (-1, 0);
                        }
                        break;
                    case ConsoleKey.RightArrow:
                        return (0, 1);
                    case ConsoleKey.LeftArrow:
                        return (0, -1);
                    case ConsoleKey.Enter:
                        _components[i].Pressed();
                        // If component is not a button we set the args index to be the value inside the component. If it is a button we run the pressed method for button with the current args.
                        if (_components[i] is not Button)
                        {
                            args[i] = _components[i].Value;
                        }
                        else if (_components[i] is Button button)
                        {
                            button.Pressed(args);
                            return;
                        }
                        break;
                }
            }
        }

        public override void Render()
        {
            for (int j = 0; j < _components.Count; j++)
            {
                _components[j].X = X;
                _components[j].Y = Y + j;
                _components[j].Render();
            }
        }
    }
}
