
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
        public InputField(string descriptor, bool isPassword, int maxLength,
            ColourFG descriptorTextColour = ColourFG.None, ColourBG descriptorBGColour = ColourBG.None,
            ColourFG inputBoxTextColour = ColourFG.None, ColourBG inputBoxBGColour = ColourBG.None)
        {
            Descriptor = descriptor;
            MaxLength = maxLength;
            IsPassword = isPassword;
            InputtedValue = string.Empty;
            _descriptorTextColour = descriptorTextColour;
            _descriptorBGColour = descriptorBGColour;
            _inputBoxTextColour = inputBoxTextColour;
            _inputBoxBGColour = inputBoxBGColour;
        }

        public override void Pressed()
        {
            InputtedValue = string.Empty;
            Render();
            Console.SetCursorPosition(X + Descriptor.Length + 3, Y); // 3, because ": ["
            Console.CursorVisible = true;
            InputtedValue = Writing();
            Value = InputtedValue;
            Console.CursorVisible = false;
        }

        public override void Measure()
        {
            if (ParentElement != null)
            {
                Width = ParentElement.Width;
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
            if (ParentElement != null)
            {
                inputBoxWidth = ParentElement.Width - Descriptor.Length - 4;
            }

            ColourManager.Set(_descriptorTextColour);
            ColourManager.Set(_descriptorBGColour);
            Console.Write($"{Descriptor}: [");

            if (_descriptorTextColour != ColourFG.None || _inputBoxTextColour != ColourFG.None)
            {
                ColourManager.Set(ColourFG.Reset);
            }

            if (_descriptorBGColour != ColourBG.None || _inputBoxBGColour != ColourBG.None)
            { 
                ColourManager.Set(ColourBG.Reset);
            }

            // Start out by filling the input box with underscores.
            for (int i = 0; i < inputBoxWidth; i++)
            {
                ColourManager.Write("_", _inputBoxTextColour, _inputBoxBGColour);
            }

            // If the user has inputted data before.
            if (!string.IsNullOrEmpty(InputtedValue))
            {
                Console.SetCursorPosition(X + Descriptor.Length + 3, Y);
                if (IsPassword)
                {
                    for (int i = 0; i < inputBoxWidth; i++)
                    {
                        ColourManager.Write("*", _inputBoxTextColour, _inputBoxBGColour);
                    }
                }
                else
                {
                    string contentsOfInputBox =
                        (InputtedValue ?? "")
                        .PadRight(inputBoxWidth, '_') // Pad possible empty spaces in the input field with underscores
                        .Substring(0, inputBoxWidth); // If the inputted string is too long, truncate it
                    ColourManager.Write(contentsOfInputBox, _inputBoxTextColour, _inputBoxBGColour);
                }
            }
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
                                    ColourManager.Write("*", _inputBoxTextColour, _inputBoxBGColour);
                                }
                                else
                                {
                                    ColourManager.Write(pressed.KeyChar.ToString(), _inputBoxTextColour, _inputBoxBGColour);
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
    }
}
