
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bank_app.Utility.Components
{
    internal class InputField //: DisplayField
    {
        // public int ElementWidth could be inherited from DisplayField
        public int ElementWidth { get; set; } = 23;
        public string Descriptor { get; private set; }
        public int MaxLength { get; private set; }
        public bool IsPassword { get; private set; }
        public string RenderedText { get; private set; }
        public string InputtedValue { private get; set; }


        /// <summary>
        /// TextInputField components have a descriptor and an input field.
        /// </summary>
        /// <param name="descriptor">The text before the user's input field.</param>
        /// <param name="isPassword">Whether the input field is a password.</param>
        /// <param name="maxLength">The amount of bytes allowed in the input field.</param>
        public InputField(string descriptor, bool isPassword, int maxLength)
        {
            Descriptor = descriptor;
            MaxLength = maxLength;
            IsPassword = isPassword;
            RenderedText = string.Empty;
            InputtedValue = string.Empty;
            // Don't render on screen here
        }

        //internal override void Clicked()
        internal void Clicked()
        {
            // TODO: Set cursor to the right position.
            Console.CursorVisible = true;
            string userInputtedString = Writing();
            RenderElement();
            Console.CursorVisible = false;
        }

        //internal override void RenderElement()
        internal void RenderElement()
        {
            /* TODO: The element renders itself in its own unique way
             * 
             * 
             * Render:     Descriptor: [_________]
             *                       123         4   <-- extra characters
             *                       
             *                       
             * Something like the below maybe?
             * 
             */
            int inputBoxWidth = ((ElementWidth - Descriptor.Length) - 4); // 4 refers to the extra characters


            Console.Write($"{Descriptor}: [");
            if (String.IsNullOrEmpty(InputtedValue)) // If user hasn't inputted anything yet.
            {
                for (int i = 0; i < inputBoxWidth; i++)
                {
                    Console.Write("_");
                }
            }
            else // User has made input.
            {
                if (IsPassword)
                {
                    for (int i = 0; i < inputBoxWidth; i++)
                    {
                        Console.Write("*");
                    }
                }
                else
                {
                    string contentsOfInputBox = (InputtedValue ?? "").PadRight(inputBoxWidth).Substring(0, inputBoxWidth);
                    Console.Write(contentsOfInputBox);
                }
            }
            Console.Write("]");
        }


        /// <summary>
        /// Allows user to input a string value while displaying input on screen. Returns inputted string,
        /// allowing no whitespace or null input. 
        /// </summary>
        /// <param name="isPassword">Show password-style stars (****) when inputting characters instead of the actual input</param>
        /// <param name="maxLength">How many characters the input can be. Set 0 for no limit</param>
        /// <returns>User's string input</returns>
        private string Writing()
        {
            string userInput = "";
            int characters = 0;
            bool isInputting = true;
            while (isInputting)
            {
                ConsoleKeyInfo pressed = Console.ReadKey(true);
                switch (pressed.Key)
                {
                    case ConsoleKey.Enter:
                        if (!String.IsNullOrEmpty(userInput))
                        {
                            isInputting = false;
                        }
                        break;

                    case ConsoleKey.Backspace:
                        if (characters > 0)
                        {
                            userInput = userInput.Substring(0, userInput.Length - 1);
                            if (IsPassword)
                            {
                                Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                                Console.Write(" ");
                                Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                            }
                            characters--;
                        }
                        break;

                    default:
                        // Only allow lowercase, UPPERCASE, numbers, Nordic extra characters and normal special characters which only
                        // require a single key press.
                        if (Regex.IsMatch(pressed.KeyChar.ToString(), @"[a-zA-Z0-9åäöæøðþÅÄÖÆØÐÞ!@#$%&*()\-_=\+\[\]{}\\|;:',.<>/?¤€£]"))
                        {
                            if (characters <= MaxLength || MaxLength == 0)
                            {
                                userInput += pressed.KeyChar;
                                characters++;
                                if (IsPassword)
                                {
                                    Console.Write("*");
                                }
                            }
                            else
                            {
                                Console.Beep();
                            }
                        }
                        break;
                }
            }
            return userInput;
        }
    }
}
