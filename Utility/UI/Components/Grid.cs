using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bank_app.Utility.UI.Components
{
    internal class Grid : UIComponent
    {
        // Row is first index, Column is second index
        private GridCell[,] _grid { get; set; }
        public int Rows { get; set; }
        public int Cols { get; set; }
        public int RowHeight { get; set; }
        public int ColWidth { get; set; }
        /// <summary>
        /// A console UI layout container that divides its available area into a 2D matrix of <see cref="GridCell"/>s.
        /// Each cell hosts one or more <see cref="UIComponent"/>s. Indexing is 0-based and ordered as [row, column].
        /// </summary>
        /// <remarks>
        /// - When measured, the grid sizes itself to its parent and computes <see cref="RowHeight"/> and <see cref="ColWidth"/> per cell.
        /// - Adding a nested <see cref="Grid"/> into a cell will automatically set the nested grid's row/column sizes
        ///   to match the host cell's dimensions.
        /// </remarks>
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
                    gridCell.ParentComponent = this;
                    gridCell.Height = RowHeight;
                    gridCell.Width = ColWidth;
                    _grid[r, c] = gridCell;
                }
            }
        }
        public override void Measure()
        {
            if (ParentComponent != null)
            {
                // Use parentelement to get row height and col width
                Width = ParentComponent.Width - 2;
                Height = ParentComponent.Height - 2;
                RowHeight = Height / Rows;
                ColWidth = Width / Cols;
                // Initialize each cell with an empty list
                for (int r = 0; r < Rows; r++)
                {
                    for (int c = 0; c < Cols; c++)
                    {
                        _grid[r, c].Height = RowHeight;
                        _grid[r, c].Width = ColWidth;
                    }
                }
            }
        }

        // Adds a component to a GridCell
        public GridCell AddGridComponent(int rowIndex, int colIndex, UIComponent component)
        {
            // Set the component ParentElement as this grid and if the component is a grid we set the RowHeight and ColWidth to one gridcell in this grids row and column system.
            component.ParentComponent = _grid[rowIndex, colIndex];
            if (component is Grid grid)
            {
                grid.RowHeight = RowHeight / grid.Rows;
                grid.ColWidth = ColWidth / grid.Cols;
            }
            // Add component to the specified row and column index
            _grid[rowIndex, colIndex].Components.Add(component);
            return _grid[rowIndex, colIndex];
        }

        // Gets a GridCell
        public GridCell GetGridCell(int rowIndex, int colIndex)
        {
            return _grid[rowIndex, colIndex];
        }
        private GridCell FindClosestGridCell(List<GridCell> gridCells, GridCell currentCell, int dX, int dY)
        {
            var candidates = gridCells.Where(cells =>
            {
                if (cells == currentCell)
                {
                    return false;
                }
                int diffX = currentCell.X - cells.X;
                int diffY = currentCell.Y - cells.Y;

                // Down direction
                if (diffX < 0 && dX == 1)
                {
                    return true;
                }
                // Up direction
                else if (diffX > 0 && dX == -1)
                {
                    return true;
                }
                // Right direction
                if (diffY < 0 && dY == 1)
                {
                    return true;
                }
                // Left direction
                else if (diffY > 0 && dY == -1)
                {
                    return true;
                }
                return false;

            });

            var closest = candidates.OrderBy(cells =>
            {
                int dxDist = cells.X - currentCell.X;
                int dyDist = cells.Y - currentCell.Y;

                return (dxDist * dxDist) + (dyDist * dyDist);
            }).FirstOrDefault();
            return closest;
        }
        public override (int, int) Pressed()
        {
            List<GridCell> interactableGridCells = new List<GridCell>();
            GridCell? currentGridCell = null;
            GridCell? cellBuffer = null;
            // Add any GridCells that has an interactable component inside it
            for (int r = 0; r < _grid.GetLength(0); r++)
            {
                for (int c = 0; c < _grid.GetLength(1); c++)
                {
                    if (_grid[r, c].Components.Any(component => component.IsInteractable))
                    {
                        interactableGridCells.Add(_grid[r, c]);
                    }
                }
            }
            if (interactableGridCells.Count > 0)
            {
                currentGridCell = interactableGridCells[0];
            }
            if (currentGridCell != null)
            {
                while (true)
                {
                    // Press the Gridcell
                    (int,int) coordinates = currentGridCell.Pressed();
                    cellBuffer = FindClosestGridCell(interactableGridCells,currentGridCell,coordinates.Item1, coordinates.Item2);
                    if (cellBuffer == null)
                    {
                        return coordinates;
                    }
                    else
                    {
                        currentGridCell.Render();
                        currentGridCell = cellBuffer;
                    }
                }
            }
            return (0, 0);
        }
        public override void Render()
        {
            // Get max columns and max rows cursor can move to
            int bufferW = Console.BufferWidth;
            int bufferH = Console.BufferHeight;

            // Render every GridCell
            for (int r = 0; r < _grid.GetLength(0); r++)
            {
                for (int c = 0; c < _grid.GetLength(1); c++)
                {
                    // Make sure we are not over max rows and columns by returning a value between 0 and buffer -1
                    int left = Math.Clamp(c * ColWidth + X, 0, bufferW - 1);
                    int top = Math.Clamp(r * RowHeight + Y, 0, bufferH - 1);
                    _grid[r, c].X = left;
                    _grid[r, c].Y = top;
                    Console.SetCursorPosition(left, top);
                    _grid[r, c].Render();
                }
            }
        }
    }
}
