using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace bank_app.Utility.Components
{
    /// <summary>
    /// Text UIComponent. Displays supplied text on screen.
    /// </summary>
    public class Text : UIComponent
    {
        private TextAlign _textAlign;
        private string[] _words;
        private List<string> _fullLines;
        private List<string> _thisLine;
        private int _totalLines;

        /// <summary>
        /// Text UIComponent constructor. Text components only contain text and are not interactable.
        /// </summary>
        /// <param name="textContents">The text to be displayed in the component.</param>
        /// <param name="textContents">Left/Center/Right alignment of text.</param>
        public Text(string textContents, TextAlign textAlign = TextAlign.Left)
        {
            _words = textContents.Split(' ');
            _fullLines = new List<string>();
            _thisLine = new List<string>();
            _textAlign = textAlign;
        }
        
        public override void Measure()
        {
            WrapIntoLines();
            // Look for longest index in words[] and set height and width
            for (int i = 0; i < _fullLines.Count; i++)
            {
                if (_fullLines[i].Length > Width)
                {
                    Width = _fullLines[i].Length;
                }
            }

            Height = _totalLines;
        }

        public override void Render()
        {
            int line = 0;
            int leftMargin = 0;
            int PanelWidth = ParentElement!.Width;

            switch (_textAlign)
            {
                case TextAlign.Left:
                    foreach (string element in _fullLines)
                    {
                        Console.SetCursorPosition(X, Y + line);
                        line = PrintElement(element, line);
                    }
                    break;

                case TextAlign.Center:
                    foreach (string element in _fullLines)
                    {
                        leftMargin = ((PanelWidth - element.Length) / 2);
                        Console.SetCursorPosition(X, Y + line);

                        for (int i = 1; i < leftMargin; i++)
                        {
                            Console.Write(" ");
                        }

                        line = PrintElement(element, line);
                    }
                    break;

                case TextAlign.Right:
                    foreach (string element in _fullLines)
                    {
                        leftMargin = (PanelWidth - element.Length);
                        Console.SetCursorPosition(X, Y + line);

                        for (int i = 1; i < leftMargin; i++)
                        {
                            Console.Write(" ");
                        }

                        line = PrintElement(element, line);
                    }
                    break;
            }
        }

        private static int PrintElement(string element, int line)
        {
            Console.Write(element);
            line++;
            return line;
        }

        private void WrapIntoLines()
        {
            int currentWidth = 0;
            int currentLine = 1;
            int PanelWidth = ParentElement!.Width;

            for (int i = 0; i < _words.Length; i++)
            {
                string word = _words[i];

                if ((currentWidth + _words[i].Length) < PanelWidth)
                {
                    _thisLine.Add($"{word}");
                    currentWidth += _words[i].Length + 1;
                    HandleIfLastWord(i);
                }
                else
                {
                    _fullLines.Add(string.Join(" ", _thisLine));
                    _thisLine.Clear();
                    currentLine++;
                    _thisLine.Add($"{word}");
                    currentWidth = _words[i].Length + 1; // Space character
                    HandleIfLastWord(i);
                }                
            }
            _totalLines = currentLine;
        }

        private void HandleIfLastWord(int i)
        {
            if (i + 1 == _words.Length)
            {
                _fullLines.Add(string.Join(" ", _thisLine));
            }
        }
    }
}
