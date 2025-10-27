using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace bank_app.Utility.Components
{
    public class Text : UIComponent
    {
        public string TextContents { get; private set; }
        private string[] _words;
        private List<string> _renderableContents;
        private int _totalLines;

        /// <summary>
        /// Text components only contain text.
        /// </summary>
        /// <param name="textContents">The text to be displayed in the component.</param>
        public Text(string textContents)
        {
            TextContents = textContents;
            _words = textContents.Split(' ');
            _renderableContents = new List<string>();
            // Split textContents into string[] with new index for every row


            int currentWidth = 0;
            int currentLine = 0;
            int PanelWidth = ParentElement.Width;

            // Word wrap TextContents. +1 handles spaces.
            for (int i = 0; i < _words.Length; i++)
            {
                if ((currentWidth + _words[i].Length) < PanelWidth)
                {
                    _renderableContents.Add($"{_words[i]} ");
                    //Console.Write($"{_words[i]} ");
                    currentWidth = currentWidth + _words[i].Length + 1;
                }
                else
                {
                    currentLine++;
                    //Console.SetCursorPosition(X, Y + currentLine);
                    _renderableContents.Add($"\n{_words[i]} ");
                    //Console.Write($"{_words[i]} ");
                    currentWidth = _words[i].Length + 1;
                }
                _totalLines = currentLine;
            }
        }
        
        public override void Measure(int parentWidth, int parentHeight)
        {
            // Look for longest index in words[] and set height and width
            for (int i = 0; i < _renderableContents.Count; i++)
            {
                if (_renderableContents[i].Length > Width)
                {
                    Width = _renderableContents[i].Length;
                }
            }

            Height = _totalLines;
        }

        public override void Render()
        {

        }
    }
}
