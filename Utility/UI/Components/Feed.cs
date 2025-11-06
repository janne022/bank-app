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

namespace bank_app.Utility.UI.Components
{
    internal class Feed : UIComponent
    {

        private List<FeedColumn> FeedColumns;
        public int _lineAmount;
        private bool _renderHeader;
        private bool _displayFooter;
        private int _pastEntries;
        public int _currentIndex;
        private ColourBG _bgColour;
        private ColourFG _textColour;
        public ColourFG _focusColour;
        private readonly IInvokable ?_invokable;
        private int _lengthOfColumns;
        private int _startIndex;
        private int _endIndex;
        private int _extraX;


        public Feed(int linesToDisplay, bool renderHeader, bool renderFooter, ColourFG textColour = ColourFG.None,
            ColourBG backgroundColour = ColourBG.None, ColourFG focusColour = ColourFG.Black, int marginTop = 0, int marginLeft = 0, int marginRight = 0, int marginBottom = 0) : base(marginTop, marginLeft, marginRight, marginBottom)
        {
            FeedColumns = new List<FeedColumn>();
            IsInteractable = true;
            IsMultiComponent = true;
            _pastEntries = 0;
            _lineAmount = linesToDisplay;
            _renderHeader = renderHeader;
            _displayFooter = renderFooter;
            _textColour = textColour;
            _bgColour = backgroundColour;
            _focusColour = focusColour;
            _currentIndex = 0;
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


        public override (int, int) Pressed(params object[] args)
        {
            while (true)
            {
                Console.CursorVisible = false;
                ConsoleKeyInfo pressed = Console.ReadKey(true);
                switch (pressed.Key)
                {
                    case ConsoleKey.Enter:
                        if (args.Length > 0)
                        {
                            _invokable?.Invoke(args, FeedColumns[0]._entries[_currentIndex]);
                        }
                        return (0, 0);

                    case ConsoleKey.Escape:
                        return (0, 0);

                    case ConsoleKey.UpArrow:
                        Scroll(UpOrDown.Up);
                        break;

                    case ConsoleKey.DownArrow:
                        Scroll(UpOrDown.Down);
                        break;
                }
                Render();
            }
        }

        public override (int, int) Pressed()
        {
            return Pressed("janne was here");
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

            ColourManager.Write($"{_startIndex + 1}-{Math.Min(_startIndex + (_endIndex - _startIndex), _lengthOfColumns)} " +
                $"of {_lengthOfColumns}.", _textColour, _bgColour);
        }
    }
}
