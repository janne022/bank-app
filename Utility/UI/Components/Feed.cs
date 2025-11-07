using bank_app.Models.Users;
using bank_app.Utility.UI.Invokables;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace bank_app.Utility.UI.Components
{
    internal class Feed : UIComponent
    {

        private List<FeedColumn> FeedColumns;
        public int _lineAmount;
        private bool _renderHeader;
        private bool _displayFooter;
        public int _currentIndex;
        private ColourBG _bgColour;
        private ColourFG _textColour;
        public ColourFG _focusColour;
        private int _lengthOfColumns;
        private int _startIndex;
        private int _endIndex;
        private int _extraX;


        public Feed(int linesToDisplay, bool renderHeader, bool renderFooter, ColourFG textColour = ColourFG.None,
            ColourBG backgroundColour = ColourBG.None, ColourFG focusColour = ColourFG.Black, 
            int marginTop = 0, int marginLeft = 0, int marginRight = 0, int marginBottom = 0) 
            : base(marginTop, marginLeft, marginRight, marginBottom)
        {
            FeedColumns = new List<FeedColumn>();
            IsMultiComponent = true;
            _lineAmount = linesToDisplay;
            _renderHeader = renderHeader;
            _displayFooter = renderFooter;
            _textColour = textColour;
            _bgColour = backgroundColour;
            _focusColour = focusColour;
            _currentIndex = -1;
            _startIndex = 0;
            _endIndex = (_startIndex + _lineAmount);
            _extraX = 0;
        }


        private int FindLowestLineAmount()
        {
            if (_lineAmount > _lengthOfColumns)
            {
                _lineAmount = _lengthOfColumns;
                return (_startIndex + _lengthOfColumns);
            }
            else
            {
                return (_startIndex + _lineAmount);
            }
        }


        public override void Measure()
        {
            Height = FeedColumns.Sum(r => r.Height) + MarginTop + MarginBottom;
            Width = FeedColumns.Sum(c => c.Width) + MarginLeft + MarginRight;
        }

        public void Scroll(UpOrDown direction)
        {
            if (direction == UpOrDown.Up)
            {
                if (_currentIndex > 0)
                {
                    _currentIndex--;

                    if (_currentIndex < _startIndex + 1)
                    {
                        if (_startIndex > 0)
                        {
                            _startIndex--;
                            _endIndex--;
                        }
                    }
                }
            }

            if (direction == UpOrDown.Down)
            {
                if (_currentIndex < _lengthOfColumns - 1)
                {
                    _currentIndex++;

                    if (_currentIndex >= _endIndex - 1)
                    {
                        if (_endIndex < _lengthOfColumns)
                        {
                            _startIndex++;
                            _endIndex++;
                        }
                    }

                }
            }
        }


        public override (int, int) Pressed()
        {
            if (_currentIndex == -1)
            {
                _currentIndex = 0;
                Render();
            }

            while (true)
            {
                Console.CursorVisible = false;
                ConsoleKeyInfo pressed = Console.ReadKey(true);
                switch (pressed.Key)
                {
                    case ConsoleKey.Enter:
                        return (-1, 0);

                    case ConsoleKey.Escape:
                        return (-1, 0);

                    case ConsoleKey.UpArrow:
                        if (_currentIndex == 0)
                        {
                            _currentIndex = -1;
                            return (-1, 0);
                        }
                        Scroll(UpOrDown.Up);
                        break;

                    case ConsoleKey.DownArrow:
                        Scroll(UpOrDown.Down);
                        break;
                }
                Render();
            }
        }

        public void AddColumn(FeedColumn incomingColumn)
        {
            incomingColumn.Measure();
            FeedColumns.Add(incomingColumn);
        }

        public override void Render()
        {
            _lengthOfColumns = FeedColumns[0]._entries.Length;
            _endIndex = FindLowestLineAmount();

            _extraX = 0;
            for (int i = 0; i < FeedColumns.Count; i++)
            {
                FeedColumns[i].X = X + _extraX;
                FeedColumns[i].Y = Y;
                FeedColumns[i].CurrentIndex = _currentIndex;
                FeedColumns[i].StartIndex = _startIndex;
                FeedColumns[i].EndIndex = _endIndex;
                _extraX += FeedColumns[i].Width;
                FeedColumns[i].Render();
            }

            if (_displayFooter)
            {
                RenderFooter();
            }
        }

        private void RenderFooter()
        {
            int footerDepth = 0;
            int headerTitleAndSpacer = 0;

            if (_renderHeader)
            {
                int headerTitle = 1;
                int headerSpacer = 1;
                headerTitleAndSpacer += headerTitle + headerSpacer;
            }

            int footerSpacer = 1;

            footerDepth += _lineAmount + headerTitleAndSpacer + footerSpacer;

            Console.SetCursorPosition(X, Y + footerDepth);

            string footerText = $"{_startIndex + 1}-{Math.Min(_startIndex + (_endIndex - _startIndex), _lengthOfColumns)} of {_lengthOfColumns}";

            int footerLeftSide = (Width - footerText.Length) / 2;
            if (footerLeftSide < 0)
            {
                footerLeftSide = 0;
            }

            int footerRightSide = Width - footerLeftSide - footerText.Length;
            if (footerRightSide < 0)
            {
                footerRightSide = 0;
            }

            Console.Write(new string(' ', footerLeftSide));
            ColourManager.Write(footerText, _textColour, _bgColour);
            Console.Write(new string(' ', footerRightSide));
        }
    }
}
