namespace bank_app.Utility.UI.Components
{
    internal class Grid : UIComponent
    {
        // Row is first index, Column is second index
        private Flexbox[,] _grid { get; set; }
        public int Rows { get; set; }
        public int Cols { get; set; }
        public int RowHeight { get; set; }
        public int ColWidth { get; set; }
        List<Flexbox> _interactableGridCells = new List<Flexbox>();
        Flexbox? _currentGridCell = null;
        /// <summary>
        /// A console UI layout container that divides its available area into a 2D matrix of <see cref="Flexbox"/>s.
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
            _grid = new Flexbox[rows, columns];
            // Since we cant be sure grid has a parent, assume default height and width of console size. Parent can change childs rowheight and colwidth
            RowHeight = Console.WindowHeight / rows;
            ColWidth = Console.WindowWidth / columns;
            IsMultiComponent = true;
            // Initialize each cell with an empty list
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < columns; c++)
                {
                    Flexbox gridCell = new Flexbox();
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
        public Flexbox AddGridComponent(int rowIndex, int colIndex, UIComponent component)
        {
            _grid[rowIndex, colIndex].AddFlexComponent(component);
            return _grid[rowIndex, colIndex];
        }

        // Gets a GridCell
        public Flexbox GetGridCell(int rowIndex, int colIndex)
        {
            return _grid[rowIndex, colIndex];
        }

        // Finds cloesest gridcell/flexbox by checking directions.
        private Flexbox FindClosestGridCell(List<Flexbox> gridCells, Flexbox currentCell, int dX, int dY)
        {
            var candidates = gridCells.Where(cell =>
            {
                if (cell == currentCell)
                {
                    return false;
                }

                // Find out if cell is to the right/left or above/below the currentCell
                int dx = cell.X - currentCell.X;
                int dy = cell.Y - currentCell.Y;

                // Left
                if (dY == -1)
                {
                    return dx < 0;
                }
                // Right
                else if (dY == 1)
                {
                    return dx > 0;
                }
                // Up
                if (dX == -1)
                {
                    return dy < 0;
                }
                // Down
                else if (dX == 1)
                {
                    return dy > 0;
                }

                return false;
            });

            // Sorts list by euclidean distance squared so that they are order from closest to furthest. Then returns the closest flexbox
            var closest = candidates.OrderBy(cell =>
            {
                int dx = cell.X - currentCell.X;
                int dy = cell.Y - currentCell.Y;
                return (dx * dx) + (dy * dy);
            }).FirstOrDefault();

            return closest;
        }
        public override (int, int) Pressed()
        {
            Flexbox? _cellBuffer = null;
            // Add any GridCells that has an interactable component inside it
            for (int r = 0; r < _grid.GetLength(0); r++)
            {
                for (int c = 0; c < _grid.GetLength(1); c++)
                {
                    if (_grid[r, c].Components.Any(component => component.IsInteractable || component.IsMultiComponent))
                    {
                        _interactableGridCells.Add(_grid[r, c]);
                    }
                }
            }
            if (_interactableGridCells.Count > 0 && _currentGridCell == null)
            {
                _currentGridCell = _interactableGridCells[0];
            }
            if (_currentGridCell != null)
            {
                while (true)
                {
                    // Press the Gridcell
                    (int, int) directionInt = _currentGridCell.Pressed();
                    _cellBuffer = FindClosestGridCell(_interactableGridCells, _currentGridCell, directionInt.Item1, directionInt.Item2);
                    if (_cellBuffer == null)
                    {
                        return directionInt;
                    }
                    else
                    {
                        _currentGridCell.Render();
                        _currentGridCell = _cellBuffer;
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
