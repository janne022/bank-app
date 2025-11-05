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

        /// <summary>
        /// Gridcell represents a a cell within a grid. Initiates a new empty list of UIComponent and sets default values for Justify, Align, OrderBy
        /// </summary>
        public Flexbox(Justify justify = Justify.Start, Align align = Align.Top, OrderBy orderBy = OrderBy.Column)
        {
            Justify = justify;
            Align = align;
            OrderBy = orderBy;
            Components = new List<UIComponent>();
        }

        public override (int, int) Pressed()
        {
            _index = Components.FindIndex(component => component.IsInteractable);
            while (true)
            {
                for (int i = 0; i < Components.Count; i++)
                {
                    if (i == _index)
                    {
                        if (Components[i].IsInteractable)
                        {
                            return Components[i].Pressed();
                        }
                    }
                }
                // Read key and if user presses up or down we add or subtract from i. If user presses Enter we run the components pressed method.
                ConsoleKey key = Console.ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.DownArrow:
                        if (_index < Components.Count - 1)
                        {
                            _index = Components.FindIndex(component => component.IsInteractable && Components.IndexOf(component) > _index);
                            if (_index == -1)
                            {
                                return (1, 0);
                            }
                        }
                        break;
                    case ConsoleKey.UpArrow:
                        if (_index > 0)
                        {
                            _index = Components.FindIndex(component => component.IsInteractable && Components.IndexOf(component) < _index);
                            if (_index == -1)
                            {
                                return (-1, 0);
                            }
                        }
                        break;
                    case ConsoleKey.RightArrow:
                        return (0, 1);
                    case ConsoleKey.LeftArrow:
                        return (0, -1);
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
