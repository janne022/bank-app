using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bank_app.Utility.Components
{
    // Currently does not render 100% correctly in terms of cursorposition
    public class AsciiArt : UIComponent
    {
        public string TextContents { get; private set; }
        private string[] _lines;

        /// <summary>
        /// Text components only contain text.
        /// </summary>
        /// <param name="textContents">The text to be displayed in the component.</param>
        public AsciiArt(string textContents)
        {
            TextContents = textContents;
            _lines = textContents.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            Width = _lines[0].Length;
        }

        public override void Render()
        {
            for (int i = 0; i < _lines.Length; i++)
            {
                Console.SetCursorPosition(X, Y + i);
                Console.Write(_lines[i]);
            }
        }
    }
}
