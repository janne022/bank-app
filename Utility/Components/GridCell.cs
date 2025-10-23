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
                switch (Justify)
                {
                    case Justify.Start:
                        Components[i].X = X;
                        break;
                    case Justify.Center:
                        Components[i].X = X + (Width / 2);
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
                        Components[i].Y = Y + Height;
                        break;
                }
                if (OrderBy == OrderBy.Row)
                {
                    Console.SetCursorPosition(Components[i].X + (i + Components[i].Width), Components[i].Y);
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
