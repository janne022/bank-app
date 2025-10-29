using bank_app.Models;
using bank_app.Utility;
using System;
using System.Collections.Generic;
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
        /// Sets the console's ANSI colour directly.
        /// </summary>
        /// <param name="choice">Enum representing a colour</param>
        public static void Set(Colour choice)
        {
            string colour = Return(choice);
            Console.Write(colour);
        }

        /// <summary>
        /// Returns the requested ANSI colour code.
        /// </summary>
        /// <param name="choice">Enum representing a colour</param>
        /// <returns>ANSI colour code as string</returns>
        public static string Return(Colour choice)
        {
            switch (choice)
            {
                case Colour.fullReset:
                    return "\u001b[0m";

                case Colour.bgReset:
                    return "\u001b[49m";

                case Colour.bgBlack:
                    return "\u001b[40m";

                case Colour.bgBlackBright:
                    return "\u001b[100m";

                case Colour.bgBlue:
                    return "\u001b[44m";

                case Colour.bgBlueBright:
                    return "\u001b[104m";

                case Colour.bgCyan:
                    return "\u001b[46m";

                case Colour.bgCyanBright:
                    return "\u001b[106m";

                case Colour.bgGreen:
                    return "\u001b[42m";

                case Colour.bgGreenBright:
                    return "\u001b[102m";

                case Colour.bgMagenta:
                    return "\u001b[45m";

                case Colour.bgMagentaBright:
                    return "\u001b[105m";

                case Colour.bgRed:
                    return "\u001b[41m";

                case Colour.bgRedBright:
                    return "\u001b[101m";

                case Colour.bgWhite:
                    return "\u001b[47m";

                case Colour.bgWhiteBright:
                    return "\u001b[107m";

                case Colour.bgYellow:
                    return "\u001b[43m";

                case Colour.bgYellowBright:
                    return "\u001b[103m";

                case Colour.fgReset:
                    return "\u001b[39m";

                case Colour.fgBlack:
                    return "\u001b[30m";

                case Colour.fgBlackBright:
                    return "\u001b[90m";

                case Colour.fgBlue:
                    return "\u001b[34m";

                case Colour.fgBlueBright:
                    return "\u001b[94m";

                case Colour.fgCyan:
                    return "\u001b[36m";

                case Colour.fgCyanBright:
                    return "\u001b[96m";

                case Colour.fgGreen:
                    return "\u001b[32m";

                case Colour.fgGreenBright:
                    return "\u001b[92m";

                case Colour.fgMagenta:
                    return "\u001b[35m";

                case Colour.fgMagentaBright:
                    return "\u001b[95m";

                case Colour.fgRed:
                    return "\u001b[31m";

                case Colour.fgRedBright:
                    return "\u001b[91m";

                case Colour.fgWhite:
                    return "\u001b[37m";

                case Colour.fgWhiteBright:
                    return "\u001b[97m";

                case Colour.fgYellow:
                    return "\u001b[33m";

                case Colour.fgYellowBright:
                    return "\u001b[93m";

                default:
                    return "\u001b[0m";
            }
        }
    }
}
