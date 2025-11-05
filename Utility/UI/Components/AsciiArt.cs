using Figgle;
using Figgle.Fonts;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace bank_app.Utility.UI.Components
{
    // Currently does not render 100% correctly in terms of cursorposition
    public class AsciiArt : UIComponent
    {
        private string[] _lines;

        /// <summary>
        /// AsciiArt UIComponent. Renders ascii art from either a file or from string
        /// </summary>
        /// <param name="asciiType"></param>
        /// <param name="content"></param>
        public AsciiArt(AsciiType asciiType, string content)
        {
            switch (asciiType)
            {
                case AsciiType.File:
                    _lines = File.ReadAllLines(content);
                    break;
                case AsciiType.String:
                    _lines = content.Split('\n');
                    break;
            }
        }
        public override void Measure()
        {

            // Strip away ansi characters into an array by using regex replace to identify a ansi character such as \x1B[0m and replacing it with an empty string
            string[] cleanedLines = _lines.Select(line => Regex.Replace(line, @"\x1B\[[0-9;]*[a-zA-Z]", "")).ToArray();
            // Loop through the cleaned array and checks for the longest line
            int longestLine = 0;
            foreach (string item in cleanedLines)
            {
                if (item.Length > longestLine)
                {
                    longestLine = item.Length;
                }
            }
            // Set height and width
            Width = longestLine;
            Height = cleanedLines.Length + MarginTop + MarginBottom;
        }

        public override void Render()
        {
            for (int i = 0; i < _lines.Length; i++)
            {
                Console.SetCursorPosition(X, Y + i);
                ColourManager.Write(_lines[i]);
            }
        }
    }
}
