using System.Text.RegularExpressions;

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
        private InputFieldType _allowedCharacters;
        public string InputtedValue { get; private set; }
        private ColourFG _descriptorTextColour;
        private ColourFG _inputBoxTextColour;
        private ColourBG _descriptorBGColour;
        private ColourBG _inputBoxBGColour;
        private ColourFG _userInputColour;
        private int _inputBoxWidth;



        /// <summary>
        /// InputField UIComponent constructor. InputFields have a descriptor and an input box.
        /// </summary>
        /// <param name="descriptor">The text before the user's input field.</param>
        /// <param name="allowedCharacters">Whether the input field is a normal, number or password field.</param>
        /// <param name="marginLeft">Amount of spaces to the left of the descriptor element.</param>
        /// <param name="maxLength">The amount of characters allowed in the input box.</param>
        /// <param name="descriptorTextColour">Text colour for descriptor</param>
        /// <param name="descriptorBGColour">Background colour for descriptor</param>
        /// <param name="inputBoxTextColour">Text colour for input box</param>
        /// <param name="inputBoxBGColour">Background colour for input box</param>
        /// <param name="userInputColour">Text colour for the user's inputted text</param>
        /// <param name="preinputtedValue">Text already present in the input box on first render</param>
        public InputField(string descriptor, InputFieldType allowedCharacters, int maxLength,
            ColourFG descriptorTextColour = ColourFG.None, ColourBG descriptorBGColour = ColourBG.None,
            ColourFG inputBoxTextColour = ColourFG.None, ColourBG inputBoxBGColour = ColourBG.None,
            ColourFG userInputColour = ColourFG.None, string preinputtedValue = "",
            int marginTop = 0, int marginLeft = 0, int marginRight = 0, int marginBottom = 0)
            : base(marginTop, marginLeft, marginRight, marginBottom)
        {
            Descriptor = descriptor;
            MaxLength = maxLength;
            MarginLeft = marginLeft;
            _allowedCharacters = allowedCharacters;
            InputtedValue = preinputtedValue;
            _descriptorTextColour = descriptorTextColour;
            _descriptorBGColour = descriptorBGColour;
            _inputBoxTextColour = inputBoxTextColour;
            _inputBoxBGColour = inputBoxBGColour;
            _userInputColour = userInputColour;
            IsInteractable = true;
            Value = InputtedValue;
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
                Width = ParentComponent.Width + MarginLeft + MarginRight;
                Height = 1 + MarginTop + MarginBottom;
            }
        }


        public override void Render()
        {
            /*                        inputBoxWidth
             * Render:     Descriptor: [_________]
             *                       123         4   <-- extra characters                    
             */

            Console.SetCursorPosition(X, Y);
            _inputBoxWidth = MarginLeft - Descriptor.Length - 4;// 4 refers to the extra characters
            if (ParentComponent != null)
            {
                _inputBoxWidth = ParentComponent.Width - MarginLeft - Descriptor.Length - 4;
            }

            for (int i = 0; i < MarginLeft; i++)
            {
                Console.Write(' ');
            }

            ColourManager.Write($"{Descriptor}: [", _descriptorTextColour, _descriptorBGColour);

            // Start out by filling the input box with underscores.
            DrawUnderscores(0, _inputBoxWidth);


            // If the user has inputted data before.
            if (!string.IsNullOrEmpty(InputtedValue))
            {
                Console.SetCursorPosition(X + MarginLeft + Descriptor.Length + 3, Y);
                if (_allowedCharacters == InputFieldType.Password)
                {
                    for (int i = 0; i < _inputBoxWidth; i++)
                    {
                        ColourManager.Write("*", _userInputColour, _inputBoxBGColour);
                    }
                }
                else
                {
                    if (InputtedValue.Length < _inputBoxWidth)
                    {
                        ColourManager.Write(InputtedValue, _userInputColour, _inputBoxBGColour);
                        DrawUnderscores(InputtedValue.Length, _inputBoxWidth);
                    }
                    else
                    {
                        ColourManager.Write(InputtedValue[..Math.Min(InputtedValue.Length, _inputBoxWidth)],
                            _userInputColour, _inputBoxBGColour);
                    }
                }
            }
            Console.SetCursorPosition(X + MarginLeft + Descriptor.Length + _inputBoxWidth + 3, Y);
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
            int scrollOffset = 0;

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
                            characters--;
                            if (scrollOffset > 0 && characters - scrollOffset < _inputBoxWidth)
                            {
                                scrollOffset--;
                            }

                            RedrawInputBox(userInput, _inputBoxWidth, scrollOffset);
                        }
                        break;

                    default:
                        // Only allow lowercase, UPPERCASE, numbers, Nordic extra characters and normal special characters which only
                        // require a single key press.
                        if (Regex.IsMatch(pressed.KeyChar.ToString(), @"[a-zA-Z0-9åäöæøðþÅÄÖÆØÐÞ!@#$%&*()\-_=\+\[\]{}\\|;:',.<>/?¤€£]"))
                        {
                            if (characters <= MaxLength || MaxLength == 0)
                            {
                                bool shouldDraw = false;

                                userInput += pressed.KeyChar;
                                characters++;

                                if (_allowedCharacters == InputFieldType.Number)
                                {
                                    if (Regex.IsMatch(pressed.KeyChar.ToString(), @"[0-9,.]"))
                                    {
                                        shouldDraw = true;
                                    }
                                    else
                                    {
                                        userInput = userInput.Substring(0, userInput.Length - 1);
                                        characters--;
                                        Console.Beep();
                                    }
                                }

                                if (_allowedCharacters == InputFieldType.Normal || _allowedCharacters == InputFieldType.Password)
                                {
                                    shouldDraw = true;
                                }

                                if (shouldDraw && userInput.Length > _inputBoxWidth)
                                {
                                    scrollOffset = userInput.Length - _inputBoxWidth;

                                    RedrawInputBox(userInput, _inputBoxWidth, scrollOffset);
                                }
                                else if (shouldDraw && userInput.Length <= _inputBoxWidth)
                                {
                                    scrollOffset = 0;

                                    RedrawInputBox(userInput, _inputBoxWidth, scrollOffset);
                                }
                                else
                                {
                                    Console.Beep();
                                }
                            }
                            else
                            {
                                Console.Beep();
                            }
                        }
                        else
                        {
                            Console.Beep();
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

        private void RedrawInputBox(string userInput, int inputBoxWidth, int scrollOffset)
        {
            Console.CursorVisible = false;

            int startIndex = userInput.Length - inputBoxWidth - scrollOffset;
            if (startIndex < 0)
            {
                startIndex = 0;
            }

            string visibleText = string.Empty;
            if (_allowedCharacters == InputFieldType.Password)
            {
                string prePassword = userInput.Substring(scrollOffset, Math.Min(inputBoxWidth, userInput.Length - scrollOffset));
                int visibleTextLength = prePassword.Length;
                visibleText = (new string('*', visibleTextLength));
            }
            else
            {
                visibleText = userInput.Substring(scrollOffset, Math.Min(inputBoxWidth, userInput.Length - scrollOffset));
            }

            int inputStartX = X + MarginLeft + Descriptor.Length + 3;
            Console.SetCursorPosition(inputStartX, Y);

            int amountOfUnderscores = inputBoxWidth - visibleText.Length;
            ColourManager.Write(visibleText, _userInputColour, _inputBoxBGColour);

            for (int i = 0; i < amountOfUnderscores; i++)
            {
                ColourManager.Write("_", _inputBoxTextColour, _inputBoxBGColour);
            }

            Console.SetCursorPosition(inputStartX + visibleText.Length, Y);

            if (visibleText.Length >= _inputBoxWidth)
            {
                Console.CursorVisible = false;
            }
            else
            {
                Console.CursorVisible = true;
            }
        }
    }
}
