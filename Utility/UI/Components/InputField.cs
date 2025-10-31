
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
using System.Reflection.PortableExecutable;

namespace bank_app.Utility.UI.Components
{
    /// <summary>
    /// InputField UIComponent. Prompts user for input.
    /// </summary>
    internal class InputField : UIComponent
    {
        private int PanelWidth { get; set; }
        public string Descriptor { get; private set; }
        public int MaxLength { get; private set; }
        public bool IsPassword { get; private set; }
        public string InputtedValue { get; private set; }
        private ColourFG _descriptorTextColour;
        private ColourFG _inputBoxTextColour;
        private ColourBG _descriptorBGColour;
        private ColourBG _inputBoxBGColour;
        private ColourFG _userInputColour;



        /// <summary>
        /// InputField UIComponent constructor. InputFields have a descriptor and an input box.
        /// </summary>
        /// <param name="descriptor">The text before the user's input field.</param>
        /// <param name="isPassword">Whether the input field is a password.</param>
        /// <param name="maxLength">The amount of characters allowed in the input box.</param>
        /// <param name="descriptorTextColour">Text colour for descriptor</param>
        /// <param name="descriptorBGColour">Background colour for descriptor</param>
        /// <param name="inputBoxTextColour">Text colour for input box</param>
        /// <param name="inputBoxBGColour">Background colour for input box</param>
        /// <param name="userInputColour">Text colour for the user's inputted text</param>
        public InputField(string descriptor, bool isPassword, int maxLength,
            ColourFG descriptorTextColour = ColourFG.None, ColourBG descriptorBGColour = ColourBG.None,
            ColourFG inputBoxTextColour = ColourFG.None, ColourBG inputBoxBGColour = ColourBG.None,
            ColourFG userInputColour = ColourFG.None)
        {
            Descriptor = descriptor;
            MaxLength = maxLength;
            IsPassword = isPassword;
            InputtedValue = string.Empty;
            _descriptorTextColour = descriptorTextColour;
            _descriptorBGColour = descriptorBGColour;
            _inputBoxTextColour = inputBoxTextColour;
            _inputBoxBGColour = inputBoxBGColour;
            _userInputColour = userInputColour;
        }

        public override (int, int) Pressed()
        {
            InputtedValue = string.Empty;
            Render();
            Console.SetCursorPosition(X + Descriptor.Length + 3, Y); // 3, because ": ["
            Console.CursorVisible = true;
            InputtedValue = Writing();
            Value = InputtedValue;
            Console.CursorVisible = false;
            return (0, 0);
        }

        public override void Measure()
        {
            if (ParentComponent != null)
            {
                Width = ParentComponent.Width;
                Height = 1;
            }
        }


        public override void Render()
        {
            /*                        inputBoxWidth
             * Render:     Descriptor: [_________]
             *                       123         4   <-- extra characters                    
             */

            Console.SetCursorPosition(X, Y);
            int inputBoxWidth = Descriptor.Length - 4;// 4 refers to the extra characters
            if (ParentComponent != null)
            {
                inputBoxWidth = ParentComponent.Width - Descriptor.Length - 4;
            }

            ColourManager.Write($"{Descriptor}: [", _descriptorTextColour, _descriptorBGColour);

            // Start out by filling the input box with underscores.
            DrawUnderscores(0, inputBoxWidth);


            // If the user has inputted data before.
            if (!string.IsNullOrEmpty(InputtedValue))
            {
                Console.SetCursorPosition(X + Descriptor.Length + 3, Y);
                if (IsPassword)
                {
                    for (int i = 0; i < inputBoxWidth; i++)
                    {
                        ColourManager.Write("*", _userInputColour, _inputBoxBGColour);
                    }
                }
                else
                {
                    if (InputtedValue.Length < inputBoxWidth)
                    {
                        ColourManager.Write(InputtedValue, _userInputColour, _inputBoxBGColour);
                        DrawUnderscores(InputtedValue.Length, inputBoxWidth);
                    }
                    else
                    {
                        ColourManager.Write(InputtedValue[..Math.Min(InputtedValue.Length, inputBoxWidth)],
                            _userInputColour, _inputBoxBGColour);
                    }
                }
            }
            Console.SetCursorPosition(X + Descriptor.Length + inputBoxWidth + 3, Y);
            ColourManager.Write("]", _descriptorTextColour, _descriptorBGColour);
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
            bool isEscaping = false;
            bool isInputting = true;
            while (isInputting)
            {
                ConsoleKeyInfo pressed = Console.ReadKey(true);
                switch (pressed.Key)
                {
                    case ConsoleKey.Enter:
                        if (!string.IsNullOrEmpty(userInput))
                        {
                            isInputting = false;
                        }
                        break;

                    case ConsoleKey.Escape:
                        isEscaping = true;
                        isInputting = false;
                        break;

                    case ConsoleKey.Backspace:
                        if (characters > 0)
                        {
                            userInput = userInput.Substring(0, userInput.Length - 1);
                            Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                            ColourManager.Write("_", _inputBoxTextColour, _inputBoxBGColour);
                            Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
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
                                    ColourManager.Write("*", _userInputColour, _inputBoxBGColour);
                                }
                                else
                                {
                                    ColourManager.Write(pressed.KeyChar.ToString(), _userInputColour, _inputBoxBGColour);
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

            if (isEscaping)
            {
                return string.Empty;
            }
            else
            {
                return userInput;
            }
        }

        private void DrawUnderscores(int startingPosition, int endingPosition)
        {
            for (int i = startingPosition; i < endingPosition; i++)
            {
                ColourManager.Write("_", _inputBoxTextColour, _inputBoxBGColour);
            }
        }
    }
}
