using bank_app.Models;
using bank_app.Utility;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bank_app.Utility.UI
{
    /// <summary>
    /// Class which handles ANSI colour code output to the console.
    /// </summary>
    internal static class ColourManager
    {
        /// <summary>
        /// Sets the console's foreground ANSI colour directly.
        /// </summary>
        /// <param name="choice">Enum representing a colour</param>
        public static void Set(ColourFG choice)
        {
            string colour = Return(choice);
            Console.Write(colour);
        }

        /// <summary>
        /// Sets the console's background ANSI colour directly.
        /// </summary>
        /// <param name="choice">Enum representing a colour</param>
        public static void Set(ColourBG choice)
        {
            string colour = Return(choice);
            Console.Write(colour);
        }

        /// <summary>
        /// Returns the requested foreground ANSI colour code.
        /// </summary>
        /// <param name="choice">Enum representing a colour</param>
        /// <returns>ANSI colour code as string</returns>
        public static string Return(ColourFG choice)
        {
            switch (choice)
            {
                case ColourFG.None:
                    return string.Empty;

                case ColourFG.Reset:
                    return "\u001b[39m";

                case ColourFG.Black:
                    return "\u001b[30m";

                case ColourFG.BlackBright:
                    return "\u001b[90m";

                case ColourFG.Blue:
                    return "\u001b[34m";

                case ColourFG.BlueBright:
                    return "\u001b[94m";

                case ColourFG.Cyan:
                    return "\u001b[36m";

                case ColourFG.CyanBright:
                    return "\u001b[96m";

                case ColourFG.Green:
                    return "\u001b[32m";

                case ColourFG.GreenBright:
                    return "\u001b[92m";

                case ColourFG.Magenta:
                    return "\u001b[35m";

                case ColourFG.MagentaBright:
                    return "\u001b[95m";

                case ColourFG.Red:
                    return "\u001b[31m";

                case ColourFG.RedBright:
                    return "\u001b[91m";

                case ColourFG.White:
                    return "\u001b[37m";

                case ColourFG.WhiteBright:
                    return "\u001b[97m";

                case ColourFG.Yellow:
                    return "\u001b[33m";

                case ColourFG.YellowBright:
                    return "\u001b[93m";

                default: // Same as ColourFG.Reset
                    return "\u001b[39m";
            }
        }

        /// <summary>
        /// Returns the requested background ANSI colour code.
        /// </summary>
        /// <param name="choice">Enum representing a colour</param>
        /// <returns>ANSI colour code as string</returns>
        public static string Return(ColourBG choice)
        {
            switch (choice)
            {
                case ColourBG.None:
                    return string.Empty;

                case ColourBG.Reset:
                    return "\u001b[49m";

                case ColourBG.Black:
                    return "\u001b[40m";

                case ColourBG.BlackBright:
                    return "\u001b[100m";

                case ColourBG.Blue:
                    return "\u001b[44m";

                case ColourBG.BlueBright:
                    return "\u001b[104m";

                case ColourBG.Cyan:
                    return "\u001b[46m";

                case ColourBG.CyanBright:
                    return "\u001b[106m";

                case ColourBG.Green:
                    return "\u001b[42m";

                case ColourBG.GreenBright:
                    return "\u001b[102m";

                case ColourBG.Magenta:
                    return "\u001b[45m";

                case ColourBG.MagentaBright:
                    return "\u001b[105m";

                case ColourBG.Red:
                    return "\u001b[41m";

                case ColourBG.RedBright:
                    return "\u001b[101m";

                case ColourBG.White:
                    return "\u001b[47m";

                case ColourBG.WhiteBright:
                    return "\u001b[107m";

                case ColourBG.Yellow:
                    return "\u001b[43m";

                case ColourBG.YellowBright:
                    return "\u001b[103m";

                default: // Same as ColourBG.Reset
                    return "\u001b[49m";
            }
        }

        /// <summary>
        /// Directly prints coloured text on screen, resetting afterwards.
        /// </summary>
        /// <param name="text">Text to be printed on screen</param>
        /// <param name="foreground">Foreground colour. Default is reset</param>
        /// <param name="background">Background colour. Default is reset</param>
        public static void Write(string text, ColourFG foreground = ColourFG.None, ColourBG background = ColourBG.None)
        {
            string colourFG = Return(foreground);
            string colourBG = Return(background);
            string resetFG = Return(ColourFG.Reset);
            string resetBG = Return(ColourBG.Reset);
            Console.Write($"{colourFG}{colourBG}{text}{resetFG}{resetBG}");
        }
    }
}
