using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bank_app.Utility.Components
{
    public class GridCell : UIComponent
    {
        public List<UIComponent> Components { get; set; }
        public Justify Justify { get; set; }
        public Align Align { get; set; }
        public OrderBy OrderBy { get; set; }

        public GridCell()
        {
            Justify = Justify.Start;
            Align = Align.Top;
            OrderBy = OrderBy.Row;
            Components = new List<UIComponent>();
        }

        public override void Render()
        {
            for (int i = 0; i < Components.Count; i++)
            {
                // Changes coordinate of current component depending on Justify and Alignment chosen
                // TODO: Coordinate should change depending on 
                switch (Justify)
                {
                    case Justify.Start:
                        Components[i].X = X;
                        break;
                    case Justify.Center:
                        Components[i].X = X + (Width / 2) - (Components[i].Width / 2);
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
                        Components[i].Y = Y + (Height / 2);
                        break;
                    case Align.Bottom:
                        Components[i].Y = Y;
                        break;
                }
                // Render either via Row or Column
                if (OrderBy == OrderBy.Row)
                {
                    // For row we check the previous component (if it isn't the first component) and add that much space + 1 extra to current cursor position
                    int extraWidth = 0;
                    if (i !=0 )
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
