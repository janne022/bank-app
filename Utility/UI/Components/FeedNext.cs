using bank_app.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bank_app.Utility.UI.Components
{
    internal class FeedNext : UIComponent
    {
        
        private List<FeedColumn> FeedColumns;
        public int _lineAmount;
        private int _pastEntries;
        public int _currentIndex;
        private ColourBG _bgColour;
        private ColourFG _textColour;
        public ColourFG _focusColour;
        private int _startIndex;
        private int _endIndex;
        private int _latestX;


        public FeedNext(int linesToDisplay, ColourFG textColour = ColourFG.None, ColourBG backgroundColour = ColourBG.None, ColourFG focusColour = ColourFG.Black)
        {
            FeedColumns = new List<FeedColumn>();
            _pastEntries = 0;
            _lineAmount = linesToDisplay;
            _textColour = textColour;
            _bgColour = backgroundColour;
            _focusColour = focusColour;
            _currentIndex = 0;
            _startIndex = 0;
            _endIndex = (_startIndex + _lineAmount);
            _latestX = 0;
        }

        public override void Measure()
        {
            Width = ParentComponent!.Width;
            Height = ParentComponent!.Height;

        }

        public void Scroll(UpOrDown direction)
        {
            if (direction == UpOrDown.Up)
            {
                if (_currentIndex > 0)
                {
                    _currentIndex--;
                    _startIndex--;
                    _endIndex--;
                }
            }

            if (direction == UpOrDown.Down)
            {
                if (_currentIndex <= FeedColumns.Count)
                {
                    _currentIndex++;
                    _startIndex++;
                    _endIndex++;
                }
            }
        }



        //public void Scroll(UpOrDown direction)
        //{

        //    int allEntries = FeedColumns.Count;

        //    if (direction == UpOrDown.Up)
        //    {
        //        if (_currentIndex > 0)
        //        {
        //            _currentIndex--;

        //            // Scroll if need be
        //            if (_currentIndex < _startIndex)
        //            {
        //                _startIndex--;
        //                _endIndex--;
        //            }
        //        }
        //    }

        //    if (direction == UpOrDown.Down)
        //    {
        //        if (_currentIndex < allEntries - 1)
        //        {
        //            _currentIndex++;

        //            if (_currentIndex >= _startIndex + _lineAmount)
        //            {
        //                _startIndex++;
        //                _endIndex++;
        //            }
        //        }
        //    }

        //}



        //public void Scroll(UpOrDown direction)
        //{

        //    int allEntries = FeedColumns.Count;

        //    if (direction == UpOrDown.Up)
        //    {
        //        if (_currentIndex > 0)
        //        {
        //            _currentIndex--;

        //            // Scroll if need be
        //            if (_currentIndex < _pastEntries)
        //            {
        //                _pastEntries--;
        //            }
        //        }
        //    }

        //    if (direction == UpOrDown.Down)
        //    {
        //        if (_currentIndex < allEntries - 1)
        //        {
        //            _currentIndex++;

        //            if (_currentIndex >= _pastEntries + _lineAmount)
        //            {
        //                _pastEntries++;
        //            }
        //        }
        //    }

        //}

        public override (int, int) Pressed()
        {
            while (true)
            {
                Console.CursorVisible = false;
                ConsoleKeyInfo pressed = Console.ReadKey(true);
                switch (pressed.Key)
                {
                    case ConsoleKey.Enter:
                        break;

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

        public void AddColumn(FeedColumn incomingColumn)
        {
            incomingColumn.Measure();
            incomingColumn.X = _latestX;
            FeedColumns.Add(incomingColumn);
            _latestX += incomingColumn.Width;
        }

        public override void Render()
        {
            int currentWidth = 0;

            //var relevantColumns = FeedColumns
            //        .Skip(_pastEntries)
            //        .Take(_lineAmount)
            //        .ToList();

            //var relevantColumns = FeedColumns
            //        .Skip(_startIndex)
            //        .Take(_lineAmount)
            //        .ToList();
            //foreach (FeedColumn column in relevantColumns)
            //{
            //    //column.X += currentWidth;
            //    column.StartIndex = _startIndex;
            //    column.EndIndex = _endIndex;
            //    //currentWidth += column.Width;
            //    column.Render();
            //}


            //for (int i = 0; i < relevantColumns.Count; i++)
            //{
            //    relevantColumns[i].CurrentIndex = _currentIndex;
            //    relevantColumns[i].StartIndex = _startIndex;
            //    relevantColumns[i].EndIndex = _endIndex;
            //    relevantColumns[i].Render();
            //}
            
            for (int i = 0; i < FeedColumns.Count; i++)
            {
                FeedColumns[i].StartIndex = _startIndex;
                FeedColumns[i].EndIndex = _endIndex;
                FeedColumns[i].Render();
            }
        }
    }
}
