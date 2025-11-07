namespace bank_app.Utility.UI.Components
{
    public class Flexbox : UIComponent
    {
        // Declare variables
        public List<UIComponent> Components { get; set; }
        public Justify Justify { get; set; }
        public Align Align { get; set; }
        public OrderBy OrderBy { get; set; }
        private int _index = 0;
        public ColourFG SelectionFG { get; set; }
        public ColourBG SelectionBG { get; set; }

        /// <summary>
        /// One dimensional layout component.
        /// </summary>
        public Flexbox(Justify justify = Justify.Start, Align align = Align.Top, OrderBy orderBy = OrderBy.Column, ColourFG selectionFG = ColourFG.Black, ColourBG selectionBG = ColourBG.White)
        {
            Justify = justify;
            Align = align;
            OrderBy = orderBy;
            SelectionFG = selectionFG;
            SelectionBG = selectionBG;
            Components = new List<UIComponent>();
            IsMultiComponent = true;
        }
        // Finds cloesest gridcell/flexbox by checking directions.
        private UIComponent FindClosestGridCell(List<UIComponent> gridCells, UIComponent currentCell, int dX, int dY)
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
            // Find first interactable or multicomponent
            _index = Components.FindIndex(component => component.IsInteractable || component.IsMultiComponent);
            while (true)
            {
                for (int i = 0; i < Components.Count; i++)
                {
                    if (i == _index)
                    {
                        // Multi components can handle their own navigation so we just go straight into them.
                        if (Components[i].IsMultiComponent)
                        {
                            // Press the multicomponent, get where user is trying to navigate to and get closest component.
                            (int, int) coords = Components[i].Pressed();
                            UIComponent closestComponent = FindClosestGridCell(Components.Where(c => c.IsInteractable || c.IsMultiComponent).ToList(), Components[_index], coords.Item1, coords.Item2);
                            if (closestComponent != null)
                            {
                                Components[_index].Render();
                                _index = Components.IndexOf(closestComponent);
                            }
                            else
                            {
                                return (coords.Item1, coords.Item2);
                            }
                        }
                        else if (Components[i].IsInteractable)
                        {
                            // If component is interactable we highlight that item and render it, then let the flexbox handle navigation between those components
                            ColourManager.Set(SelectionBG);
                            ColourManager.Set(SelectionFG);
                            Components[i].Render();
                            ColourManager.Set(ColourBG.Reset);
                            ColourManager.Set(ColourFG.Reset);
                        }
                    }
                }
                if (Components[_index].IsInteractable)
                {
                    // Read key and if user presses up or down we add or subtract from index. If user presses Enter we run the components pressed method.
                    ConsoleKey key = Console.ReadKey(true).Key;
                    switch (key)
                    {
                        case ConsoleKey.DownArrow:
                            if (_index < Components.Count - 1)
                            {
                                _index = Components.FindIndex(component => (component.IsInteractable || component.IsMultiComponent) && Components.IndexOf(component) > _index);
                                if (_index == -1)
                                {
                                    return (1, 0);
                                }
                            }
                            break;
                        case ConsoleKey.UpArrow:
                            if (_index > 0)
                            {
                                _index = Components.FindIndex(component => (component.IsInteractable || component.IsMultiComponent) && Components.IndexOf(component) < _index);
                                if (_index == -1)
                                {
                                    return (-1, 0);
                                }
                            }
                            break;
                        case ConsoleKey.RightArrow:
                            if (_index < Components.Count - 1)
                            {
                                _index = Components.FindIndex(component => (component.IsInteractable || component.IsMultiComponent) && Components.IndexOf(component) > _index);
                                if (_index == -1)
                                {
                                    return (0, 1);
                                }
                            }
                            break;
                        case ConsoleKey.LeftArrow:
                            if (_index > 0)
                            {
                                _index = Components.FindIndex(component => (component.IsInteractable || component.IsMultiComponent) && Components.IndexOf(component) < _index);
                                if (_index == -1)
                                {
                                    return (0, -1);
                                }
                            }
                            break;
                        case ConsoleKey.Enter:
                            return Components[_index].Pressed();
                    }
                }
            }
        }

        public Flexbox AddFlexComponent(UIComponent component)
        {
            Components.Add(component);
            component.ParentComponent = this;
            if (component.IsInteractable)
            {
                IsInteractable = true;
            }
            return this;
        }

        public Flexbox SetJustify(Justify justify)
        {
            Justify = justify;
            return this;
        }

        public Flexbox SetAlign(Align align)
        {
            Align = align;
            return this;
        }

        public Flexbox SetOrderBy(OrderBy orderBy)
        {
            OrderBy = orderBy;
            return this;
        }

        public override void Measure()
        {
            if (ParentComponent != null)
            {
                if (ParentComponent is not Grid)
                {
                    Height = ParentComponent.Height;
                    Width = ParentComponent.Width;
                }
            }
            else
            {
                Height = Console.WindowHeight - 1;
                Width = Console.WindowWidth - 1;
            }
        }

        public override void Render()
        {
            int totalHeight = 1;
            int extraWidth = 0;
            for (int i = 0; i < Components.Count; i++)
            {
                Components[i].X = X;
                Components[i].Y = Y;
                Components[i].Measure();
                // Render either via Row or Column
                if (OrderBy == OrderBy.Row)
                {
                    Components[i].X += extraWidth;
                    extraWidth = Components[i - 1].Width + 1;
                }
                else if (OrderBy == OrderBy.Column)
                {
                    Components[i].Y += totalHeight;
                    totalHeight += Components[i].Height + 1;
                }
                // Changes coordinate of current component depending on Justify and Alignment chosen
                switch (Justify)
                {
                    case Justify.Center:
                        Components[i].X += ((Width / 2) - (Components[i].Width / 2));
                        break;
                    case Justify.End:
                        Components[i].X += Width;
                        break;
                }
                switch (Align)
                {
                    case Align.Middle:
                        Components[i].Y += ((Height / 2) - (Components[i].Height / 2));
                        break;
                    case Align.Bottom:
                        Components[i].Y += Height;
                        break;
                }
                Components[i].Render();
            }
        }
    }
}
