using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bank_app.Utility.Components
{
    public class Menu
    {
        /// <summary>
        /// Displays a menu with options for the user to choose from. They can navigate using arrow keys up and down. Clears console before displaying.
        /// </summary>
        /// <typeparam name="T">An array that is going to be displayed as strings</typeparam>
        /// <param name="menuOptions">Options in an array that is displayed to the user</param>
        /// <returns>Returns the index chosen by user</returns>
        public static int ReadOptionIndex<T>(T[] menuOptions)
        {
            int i = 0;
            while (true)
            {
                for (int j = 0; j < menuOptions.Length; j++)
                {
                    Console.BackgroundColor = i == j ? ConsoleColor.White : ConsoleColor.Black;
                    Console.ForegroundColor = i == j ? ConsoleColor.Black : ConsoleColor.White;
                    Console.WriteLine(menuOptions[j]);
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.White;
                }
                ConsoleKey key = Console.ReadKey().Key;
                switch (key)
                {
                    case ConsoleKey.DownArrow:
                        if (i < menuOptions.Length - 1)
                        {
                            i++;
                        }
                        break;
                    case ConsoleKey.UpArrow:
                        if (i > 0)
                        {
                            i--;
                        }
                        break;
                    case ConsoleKey.Enter:
                        // returns index as int
                        return i;

                }
            }
        }
    }
}
