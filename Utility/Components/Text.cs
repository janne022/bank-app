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
        // TODO: This component must know the width of its parent Panel.
        public int PanelWidth { get; private set; } = 30; // <--- TODO: remove that once this isn't hardcoded!
        public string TextContents { get; private set; }

        /// <summary>
        /// Text components only contain text.
        /// </summary>
        /// <param name="textContents">The text to be displayed in the component.</param>
        public Text(string textContents)
        {
            TextContents = textContents;
            Width = textContents.Length;
        }

        public override void Render()
        {
            int currentWidth = 0;
            int currentLine = 0;
            string[] words = TextContents.Split(' ');

            // Word wrap TextContents. +1 handles spaces.
            for (int i = 0; i < words.Length; i++)
            {
                if ((currentWidth + words[i].Length) < PanelWidth)
                {
                    Console.Write($"{words[i]} ");
                    currentWidth = currentWidth + words[i].Length + 1;
                }
                else
                {
                    currentLine++;
                    Console.SetCursorPosition(X, Y + currentLine);
                    Console.Write($"{words[i]} ");
                    currentWidth = words[i].Length + 1;
                }
            }
        }
    }
}
