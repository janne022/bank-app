using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bank_app.Utility.Components
{
    // Root component that should always be at the top
    public class Layout : UIComponent
    {
        // Declare variables
        public LayoutBorder Border { get; set; }
        private string _topLeftCorner = " ";
        private string _topRightCorner = " ";
        private string _bottomLeftCorner = " ";
        private string _bottomRightCorner = " ";
        private string _verticalWall = " ";
        private string _horizontalWall = " ";
        // Child element
        private UIComponent _rootComponent;

        // Initiate Constructor. Note: width and height has 0 as initial parameter because they need to be constant, so Console.WindowWidth can't be used.
        public Layout(UIComponent rootComponent, LayoutBorder border, int width = 0, int height = 0)
        {
            if (width == 0)
            {
                Width = Console.WindowWidth - 1;
            }
            else
            {
                Width = width;
            }
            if (height == 0)
            {
                Height = Console.WindowHeight - 1;
            }
            else
            {
                Height = height;
            }

            // Set rootComponent and set its parents element to this object
            _rootComponent = rootComponent;
            _rootComponent.ParentElement = this;
            _rootComponent.X++;
            _rootComponent.Y++;
            // If rootComponent is a grid we will take this objects height/width and change RowHeight & ColWidth for rootComponent
            if (rootComponent is Grid grid)
            {
                grid.RowHeight = Height / grid.Rows;
                grid.ColWidth = Width / grid.Cols;
            }
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
                        Console.Write(_topLeftCorner);
                    }
                    else if (col == Width - 1 && row == 0)
                    {
                        Console.Write(_topRightCorner);
                    }
                    else if (col == 0 && row == Height - 1)
                    {
                        Console.Write(_bottomLeftCorner);
                    }
                    else if (col == Width - 1 && row == Height - 1)
                    {
                        Console.Write(_bottomRightCorner);
                    }
                    else if (col == 0 || col == Width - 1)
                    {
                        Console.Write(_verticalWall);
                    }
                    else if (row == Height - 1 || row == 0)
                    {
                        Console.Write(_horizontalWall);
                    }
                    else
                    {
                        Console.Write(' ');
                    }
                }
            }
            _rootComponent.Render();
        }
    }
}
