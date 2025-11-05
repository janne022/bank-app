namespace bank_app.Utility.UI.Components
{
    internal class Feed : UIComponent
    {

        private List<FeedColumn> FeedColumns;
        public int _lineAmount;
        private int _pastEntries;
        public int _currentIndex;
        private ColourBG _bgColour;
        private ColourFG _textColour;
        public ColourFG _focusColour;
        private readonly IInvokable _invokable;
        private int _startIndex;
        private int _endIndex;
        private int _latestX;


        public Feed(int linesToDisplay, ColourFG textColour = ColourFG.None, ColourBG backgroundColour = ColourBG.None, ColourFG focusColour = ColourFG.Black)
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
                if (_currentIndex < FeedColumns[0]._entries.Length - 1)
                {
                    _currentIndex++;

                    if (_currentIndex >= _endIndex - 1)
                    {
                        if (_endIndex < FeedColumns[0]._entries.Length)
                        {
                            _startIndex++;
                            _endIndex++;
                        }
                    }

                }
            }
        }


        public (int, int) Pressed(params object[] args)
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
                            _invokable.Invoke(args, FeedColumns[0]._entries[_currentIndex]);
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

        public void AddColumn(FeedColumn incomingColumn)
        {
            incomingColumn.Measure();
            incomingColumn.X = _latestX;
            FeedColumns.Add(incomingColumn);
            _latestX += incomingColumn.Width;
        }

        public override void Render()
        {
            for (int i = 0; i < FeedColumns.Count; i++)
            {
                FeedColumns[i].CurrentIndex = _currentIndex;
                FeedColumns[i].StartIndex = _startIndex;
                FeedColumns[i].EndIndex = _endIndex;
                FeedColumns[i].Render();
            }
        }
    }
}
