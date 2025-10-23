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
        private GridCell[,] _grid { get; set; }
        public int Rows { get; set; }
        public int Cols { get; set; }
        public int RowHeight { get; set; }
        public int ColWidth { get; set; }
        public Grid(int rows, int columns)
        {
            Rows = rows;
            Cols = columns;
            _grid = new GridCell[rows, columns];
            // Since we cant be sure grid has a parent, assume default height and width of console size. Parent can change childs rowheight and colwidth
            RowHeight = Console.WindowHeight / rows;
            ColWidth = Console.WindowWidth / columns;
            // Initialize each cell with an empty list
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < columns; c++)
                {
                    GridCell gridCell = new GridCell();
                    gridCell.ParentElement = this;
                    gridCell.Height = RowHeight;
                    gridCell.Width = ColWidth;
                    _grid[r, c] = gridCell;
                }
            }
        }
        public GridCell AddGridComponent(int rowIndex, int colIndex, UIComponent component)
        {
            // Set the component ParentElement as this grid and if the component is a grid we set the RowHeight and ColWidth to one gridcell in this grids row and column system.
            component.ParentElement = _grid[rowIndex, colIndex];
            if (component is Grid grid)
            {
                grid.RowHeight = RowHeight / grid.Rows;
                grid.ColWidth = ColWidth / grid.Cols;
            }
            // Add component to the specified row and column index
            _grid[rowIndex, colIndex].Components.Add(component);
            return _grid[rowIndex, colIndex];
        }
        public GridCell GetGridCell(int rowIndex, int colIndex)
        {
            return _grid[rowIndex, colIndex];
        }
        public override void Render()
        {
            // Get max columns and max rows cursor can move to
            int bufferW = Console.BufferWidth;
            int bufferH = Console.BufferHeight;
            // Start by rendering every component that isnt interactable. This is done because the Interactable components will automatically pause the process.
            for (int r = 0; r < _grid.GetLength(0); r++)
            {
                for (int c = 0; c < _grid.GetLength(1); c++)
                {
                    if (!_grid[r, c].Components.Any(component => component.IsInteractable))
                    {
                        // Make sure we are not over max rows and columns by returning a value between 0 and buffer -1
                        int left = Math.Clamp((c * ColWidth) + X, 0, bufferW - 1);
                        int top = Math.Clamp((r * RowHeight) + Y, 0, bufferH - 1);
                        _grid[r, c].X = left;
                        _grid[r, c].Y = top;
                        Console.SetCursorPosition(left, top);
                        _grid[r, c].Render();
                    }
                }
            }

            // Render all components that are interactable. First component will render and next component will render once first has had its intended interaction
            for (int r = 0; r < _grid.GetLength(0); r++)
            {
                for (int c = 0; c < _grid.GetLength(1); c++)
                {
                    if (_grid[r, c].Components.Any(component => component.IsInteractable))
                    {
                        // Make sure we are not over max rows and columns by returning a value between 0 and buffer -1
                        int left = Math.Clamp(c * ColWidth, 0, bufferW - 1);
                        int top = Math.Clamp(r * RowHeight, 0, bufferH - 1);
                        _grid[r, c].X = left;
                        _grid[r, c].Y = top;
                        Console.SetCursorPosition(left, top);
                        _grid[r, c].Render();
                    }
                }
            }
        }
    }
}
