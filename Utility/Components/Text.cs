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

        /// <summary>
        /// Text components only contain text.
        /// </summary>
        /// <param name="textContents">The text to be displayed in the component.</param>
        public Text(string textContents)
        {
            TextContents = textContents;
            _words = textContents.Split(' ');
        }

        public override void Render()
        {
            int currentWidth = 0;
            int currentLine = 0;
            int PanelWidth = ParentElement.Width;

            // Word wrap TextContents. +1 handles spaces.
            for (int i = 0; i < _words.Length; i++)
            {
                if ((currentWidth + _words[i].Length) < PanelWidth)
                {
                    Console.Write($"{_words[i]} ");
                    currentWidth = currentWidth + _words[i].Length + 1;
                }
                else
                {
                    currentLine++;
                    Console.SetCursorPosition(X, Y + currentLine);
                    Console.Write($"{_words[i]} ");
                    currentWidth = _words[i].Length + 1;
                }
            }
        }
    }
}
