namespace bank_app.Utility.UI.Components
{
    public class Panel : UIComponent
    {
        // Declare variables
        public LayoutBorder Border { get; set; }
        private string _topLeftCorner = " ";
        private string _topRightCorner = " ";
        private string _bottomLeftCorner = " ";
        private string _bottomRightCorner = " ";
        private string _verticalWall = " ";
        private string _horizontalWall = " ";
        private ColourFG _borderColour;
        private ColourBG _backgroundColour;

        // Child element
        private UIComponent _childComponent;
        public string TopText { get; set; }

        /// <summary>
        /// Creates a layout container that optionally draws a border and hosts a single UI component.
        /// </summary>
        /// <param name="rootComponent">
        /// The child component to render inside the layout. Its <see cref="UIComponent.ParentComponent"/> is set to this layout.
        /// If the component is a Grid, its row and column sizes are computed from the layout size.
        /// </param>
        /// <param name="border">The border style to draw around the layout bounds.</param>
        /// <param name="topText">Optional text centered on the top border row.</param>
        /// <param name="width">
        /// Explicit layout width in characters. If 0, defaults to Console.WindowWidth - 1 to fit the current console.
        /// </param>
        /// <param name="height">
        /// Explicit layout height in characters. If 0, defaults to Console.WindowHeight - 1 to fit the current console.
        /// </param>
        /// <remarks>
        /// During rendering, the child component is measured and centered within the layout. The selected border style determines
        /// which corner and wall glyphs are used. The <paramref name="topText"/> is written on the top border if provided.
        /// </remarks>
        public Panel(UIComponent rootComponent, LayoutBorder border, string topText = "", int width = 0, int height = 0, ColourFG borderColour = ColourFG.Reset, ColourBG backgroundColour = ColourBG.Reset, int marginTop = 0, int marginLeft = 0, int marginRight = 0, int marginBottom = 0) : base(marginTop, marginLeft, marginRight, marginBottom)
        {
            _borderColour = borderColour;
            _backgroundColour = backgroundColour;
            TopText = topText;
            IsMultiComponent = true;
            if (height != 0 && width != 0)
            {
                Height = height + MarginBottom + MarginTop;
                Width = width + MarginLeft + MarginRight;
            }
            else if (ParentComponent != null)
            {
                Height = ParentComponent.Height + MarginLeft + MarginRight;
                Width = ParentComponent.Width + MarginLeft + MarginRight;
            }
            else
            {
                Height = Console.WindowHeight - 1;
                Width = Console.WindowWidth - 1;
            }

            // Set rootComponent and set its parents element to this object
            _childComponent = rootComponent;
            _childComponent.ParentComponent = this;
            // Chosen border style
            switch (border)
            {
                case LayoutBorder.None:
                    _topLeftCorner = " ";
                    _topRightCorner = " ";
                    _bottomLeftCorner = " ";
                    _bottomRightCorner = " ";
                    _verticalWall = " ";
                    _horizontalWall = " ";
                    break;
                case LayoutBorder.Ascii:
                    _topLeftCorner = "+";
                    _topRightCorner = "+";
                    _bottomLeftCorner = "+";
                    _bottomRightCorner = "+";
                    _verticalWall = "|";
                    _horizontalWall = "-";
                    break;
                case LayoutBorder.Square:
                    _topLeftCorner = "┌";
                    _topRightCorner = "┐";
                    _bottomLeftCorner = "└";
                    _bottomRightCorner = "┘";
                    _verticalWall = "│";
                    _horizontalWall = "─";
                    break;
                case LayoutBorder.Rounded:
                    _topLeftCorner = "╭";
                    _topRightCorner = "╮";
                    _bottomLeftCorner = "╰";
                    _bottomRightCorner = "╯";
                    _verticalWall = "│";
                    _horizontalWall = "─";
                    break;
                case LayoutBorder.Heavy:
                    _topLeftCorner = "┏";
                    _topRightCorner = "┓";
                    _bottomLeftCorner = "┗";
                    _bottomRightCorner = "┛";
                    _verticalWall = "┃";
                    _horizontalWall = "━";
                    break;
                case LayoutBorder.Double:
                    _topLeftCorner = "╔";
                    _topRightCorner = "╗";
                    _bottomLeftCorner = "╚";
                    _bottomRightCorner = "╝";
                    _verticalWall = "║";
                    _horizontalWall = "═";
                    break;
            }
        }
        public override (int, int) Pressed()
        {
            return _childComponent.Pressed();
        }

        // First render the border, then the rootComponent
        public override void Render()
        {
            for (int col = 0; col < Width; col++)
            {
                for (int row = 0; row < Height; row++)
                {
                    Console.SetCursorPosition(col + X, row + Y);
                    if (col == 0 && row == 0)
                    {
                        ColourManager.Write(_topLeftCorner, _borderColour);
                    }
                    else if (col == Width - 1 && row == 0)
                    {
                        ColourManager.Write(_topRightCorner, _borderColour);
                    }
                    else if (col == 0 && row == Height - 1)
                    {
                        ColourManager.Write(_bottomLeftCorner, _borderColour);
                    }
                    else if (col == Width - 1 && row == Height - 1)
                    {
                        ColourManager.Write(_bottomRightCorner, _borderColour);
                    }
                    else if (col == 0 || col == Width - 1)
                    {
                        ColourManager.Write(_verticalWall, _borderColour);
                    }
                    else if (row == Height - 1 || row == 0)
                    {
                        ColourManager.Write(_horizontalWall, _borderColour);
                    }
                    else
                    {
                        ColourManager.Write(" ", background: _backgroundColour);
                    }
                }
            }
            _childComponent.Measure();
            if (!string.IsNullOrEmpty(TopText))
            {
                Console.SetCursorPosition(X + (Width / 2) - (TopText.Length / 2), Y);
                Console.Write(TopText);
            }
            _childComponent.X = X + (Width / 2) - (_childComponent.Width / 2);
            _childComponent.Y = Y + (Height / 2) - (_childComponent.Height / 2);
            _childComponent.Render();
        }
    }
}
