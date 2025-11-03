using bank_app.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace bank_app.Utility.UI.Components
{
    internal class FeedColumn : UIComponent
    {
        private string[] _entries;
        private string _header;
        private bool _displayHeader;
        private bool _displayFooter;
        private TextAlign _textAlign;
        private ColourBG _bgColour;
        private ColourFG _textColour;
        private ColourFG _focusColour;
        private int _scrollHeight;
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }


        /// <summary>
        /// A column of properties to be displayed in a Feed.
        /// </summary>
        /// <param name="entries">The actual data as a string[]</param>
        /// <param name="header">The title of the data field</param>
        /// <param name="displayHeader">Whether to show the header</param>
        /// <param name="displayFooter">Whether to show the footer</param>
        /// <param name="textAlign">Left, center or right</param>
        public FeedColumn(string[] entries, string header,
            bool displayHeader = true, bool displayFooter = true,
            TextAlign textAlign = TextAlign.Left)
        {
            _entries = entries;
            _header = header;
            _displayHeader = displayHeader;
            _displayFooter = displayFooter;
            _textAlign = textAlign;
            StartIndex = 0;
            EndIndex = 0;
            _scrollHeight = EndIndex - StartIndex;
        }

        public override void Measure()
        {
            int actualWidth = 0;
            foreach (string entry in _entries)
            {
                if (entry.Length > actualWidth)
                {
                    actualWidth = entry.Length;
                }
            }
            int extraMargin = 2; // 2 margin to the right to look good in multi-column view
            Width = actualWidth + extraMargin;


            // Header  Header2 | - - - - - - -  +1
            // ----------------| - - - - - - -  +2
            // Entry   Entry   |----\     
            // Entry   Entry   |--------- _scrollHeight
            // Entry   Entry   |----/
            // ---             | - - - - - - -  +3
            // Footer          | - - - - - - -  +4

            int actualHeight = _scrollHeight;

            if (_displayHeader)                 
            {                                   
                int headerText = 1;             
                int headerSpacer = 1;           
                actualHeight += headerText;     
                actualHeight += headerSpacer;   
            }                                   
                                                
            if (_displayFooter)                 
            {
                int footerText = 1;
                int footerSpacer = 1;
                actualHeight += footerText;
                actualHeight += footerSpacer;
            }

            Height = actualHeight;
        }


        public override void Render()
        {
            Console.SetCursorPosition(X, Y);

            int linesDown = 0;

            if (_displayHeader)
            {
                linesDown = RenderHeader(linesDown);
            }

            for (int i = StartIndex; i < EndIndex; i++)
            {
                linesDown = RenderJustified(_entries[i], linesDown);
            }
            Console.SetCursorPosition(X, Y + linesDown + 1);
            RenderFooter(linesDown);
        }


        private int RenderJustified(string text, int linesDown)
        {
            int marginLeft = 0;
            switch (_textAlign)
            {
                case TextAlign.Right:
                    marginLeft = (Width - text.Length);
                    break;

                case TextAlign.Center:
                    marginLeft = ((Width - text.Length) / 2);
                    break;

                case TextAlign.Left:
                default:
                    marginLeft = 0;
                    break;
            }

            Console.SetCursorPosition(X, Y + linesDown);

            for (int i = 1; i < marginLeft; i++)
            {
                Console.Write(" ");
            }
            Console.Write($"{text}  "); // Spaces to look good in multi-column view
            linesDown++;
            return linesDown;
        }

        private int RenderHeader(int linesDown)
        {
            RenderJustified(_header, linesDown);
            linesDown++;

            Console.SetCursorPosition(X, Y + linesDown);
            for (int i = 0; i < Width; i++)
            {
                ColourManager.Write("─", _textColour, _bgColour);
            }
            linesDown++;

            return linesDown;
        }

        private void RenderFooter(int linesDown)
        {
            Console.SetCursorPosition(X, Y + linesDown);

            if (_displayFooter)
            {
                for (int i = 0; i < Width; i++)
                {
                    ColourManager.Write("─", _textColour, _bgColour);
                }
            }
        }
    }
}