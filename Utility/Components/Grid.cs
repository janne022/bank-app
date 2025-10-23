using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bank_app.Utility.Components
{
    internal class Grid : UIComponent
    {
        private List<UIComponent>[,] _components { get; set; }
        public Justify Justify { get; set; }
        public Align Align { get; set; }
        public OrderBy OrderBy { get; set; }
        public int Rows { get; set; }
        public int Cols { get; set; }
        public Grid(List<UIComponent> components, int rows, int columns, Justify justify = Justify.Start, Align align = Align.Top, OrderBy orderBy = OrderBy.Row)
        {
            Rows = rows;
            Cols = columns;
            Justify = justify;
            Align = align;
            OrderBy = orderBy;
            _components = new List<UIComponent>[rows,columns];

            // Initialize each cell with an empty list
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < columns; c++)
                {
                    _components[r, c] = new List<UIComponent>();
                }
            }
        }
        public override void Render()
        {

        }
    }
}
