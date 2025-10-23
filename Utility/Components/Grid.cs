using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bank_app.Utility.Components
{
    internal class Grid : UIComponent
    {
        // Row is first index, Column is second index
        private List<UIComponent>[,] _components { get; set; }
        public Justify Justify { get; set; }
        public Align Align { get; set; }
        public OrderBy OrderBy { get; set; }
        public int Rows { get; set; }
        public int Cols { get; set; }
        public int RowHeight { get; set; }
        public int ColWidth { get; set; }
        public Grid(int rows, int columns, Justify justify = Justify.Start, Align align = Align.Top, OrderBy orderBy = OrderBy.Row)
        {
            Rows = rows;
            Cols = columns;
            Justify = justify;
            Align = align;
            OrderBy = orderBy;
            _components = new List<UIComponent>[rows,columns];
            // Since we cant be sure grid has a parent, assume default height and width of console size. Parent can change childs rowheight and colwidth
            RowHeight = Console.WindowHeight / rows;
            ColWidth = Console.WindowWidth / columns;
            // Initialize each cell with an empty list
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < columns; c++)
                {
                    _components[r, c] = new List<UIComponent>();
                }
            }
        }
        public void AddGridComponent(int rowIndex, int colIndex, UIComponent component)
        {
            // Set the component ParentElement as this grid and if the component is a grid we set the RowHeight and ColWidth to one gridcell in this grids row and column system.
            component.ParentElement = this;
            if (component is Grid grid)
            {
                grid.RowHeight = RowHeight / grid.Rows;
                grid.ColWidth = ColWidth / grid.Cols;
            }
            // Add component to the specified row and column index
            _components[rowIndex,colIndex].Add(component);
        }
        public override void Render()
        {
            int bufferW = Console.BufferWidth;
            int bufferH = Console.BufferHeight;
            // Start by rendering every component that isnt interactable
            for (int r = 0; r < _components.GetLength(0); r++)
            {
                for (int c = 0; c < _components.GetLength(1); c++)
                {
                    for (int i = 0; i < _components[r, c].Count; i++)
                    {
                        if (!_components[r, c][i].IsInteractable)
                        {
                            int left = Math.Clamp((c * ColWidth) + X, 0, bufferW - 1);
                            int top = Math.Clamp((r * RowHeight) + Y, 0, bufferH - 1);
                            Console.SetCursorPosition(left, top);
                            _components[r, c][i].Render();
                        }
                    }
                }
            }

            // Render all components that are interactable
            for (int r = 0; r < _components.GetLength(0); r++)
            {
                for (int c = 0; c < _components.GetLength(1); c++)
                {
                    for (int i = 0; i < _components[r, c].Count; i++)
                    {
                        if (_components[r, c][i].IsInteractable)
                        {
                            int left = Math.Clamp(c * ColWidth, 0, bufferW - 1);
                            int top = Math.Clamp(r * RowHeight, 0, bufferH - 1);
                            Console.SetCursorPosition(left, top);
                            _components[r, c][i].Render();
                        }
                    }
                }
            }
        }
    }
}
