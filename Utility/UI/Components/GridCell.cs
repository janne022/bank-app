using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bank_app.Utility.UI.Components
{
    public class GridCell : UIComponent
    {
        // Declare variables
        public List<UIComponent> Components { get; set; }
        public Justify Justify { get; set; }
        public Align Align { get; set; }
        public OrderBy OrderBy { get; set; }

        /// <summary>
        /// Gridcell represents a a cell within a grid. Initiates a new empty list of UIComponent and sets default values for Justify, Align, OrderBy
        /// </summary>
        public GridCell()
        {
            Justify = Justify.Start;
            Align = Align.Top;
            OrderBy = OrderBy.Row;
            Components = new List<UIComponent>();
        }

        public override (int,int) Pressed()
        {
            int index = 0;
            while (true)
            {
                for (int i = 0; i < Components.Count; i++)
                {
                    // Render either via Row or Column
                    if (OrderBy == OrderBy.Row)
                    {
                        // For row we check the previous component (if it isn't the first component) and add that much space + 1 extra to current cursor position
                        int extraWidth = 0;
                        if (i != 0)
                        {
                            extraWidth = Components[i - 1].Width + 1;
                        }
                        Console.SetCursorPosition(Components[i].X + extraWidth, Components[i].Y);
                    }
                    else if (OrderBy == OrderBy.Column)
                    {
                        Console.SetCursorPosition(Components[i].X, Components[i].Y + i);
                    }
                    if (i == index)
                    {
                        if (Components[i] is Layout || Components[i] is Grid || Components[i] is Menu)
                        {
                            Components[i].Pressed();
                        }
                        else
                        {
                            Console.Write($"\u001b[30;47m");
                            Components[i].Render();
                            Console.Write("\u001b[0m");
                        }
                    }
                    else
                    {
                        Components[i].Render();
                    }
                }
                // Read key and if user presses up or down we add or subtract from i. If user presses Enter we run the components pressed method.
                ConsoleKey key = Console.ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.DownArrow:
                        return (1, 0);
                    case ConsoleKey.UpArrow:
                        return (-1, 0);
                    case ConsoleKey.RightArrow:
                        return (0, 1);
                    case ConsoleKey.LeftArrow:
                        return (0, -1);
                }
            }
        }

        public override void Render()
        {
            for (int i = 0; i < Components.Count; i++)
            {
                // Changes coordinate of current component depending on Justify and Alignment chosen
                Components[i].Measure();
                switch (Justify)
                {
                    case Justify.Start:
                        Components[i].X = X;
                        break;
                    case Justify.Center:
                        Components[i].X = X + (Width / 2 - Components[i].Width / 2);
                        break;
                    case Justify.End:
                        Components[i].X = X + Width;
                        break;
                }
                switch (Align)
                {
                    case Align.Top:
                        Components[i].Y = Y;
                        break;
                    case Align.Middle:
                        Components[i].Y = Y + Height / 2;
                        break;
                    case Align.Bottom:
                        Components[i].Y = Y + Height;
                        break;
                }
                // Render either via Row or Column
                if (OrderBy == OrderBy.Row)
                {
                    // For row we check the previous component (if it isn't the first component) and add that much space + 1 extra to current cursor position
                    int extraWidth = 0;
                    if (i != 0)
                    {
                        extraWidth = Components[i - 1].Width + 1;
                    }
                    Console.SetCursorPosition(Components[i].X + extraWidth, Components[i].Y);
                }
                else if (OrderBy == OrderBy.Column)
                {
                    Console.SetCursorPosition(Components[i].X, Components[i].Y + i);
                }
                Components[i].Render();
            }
        }
    }
}
