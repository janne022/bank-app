using System.Numerics;
using System.Text.RegularExpressions;

namespace bank_app
{
    /// <summary>
    /// A class containing methods for handling terminal user inputs.
    /// </summary>
    internal class UserInput
    {
        //
        //
        //              - = User input validations = -
        //
        //

        /// <summary>
        /// Ensures user inputs an integer, prompting until they do.
        /// </summary>
        /// <returns>Integer</returns>
        public static int Integer()
        {
            return ValidateNumber<int>(VariableType.Integer, ValueType.Any, true);
        }

        /// <summary>
        /// Ensures user inputs a positive integer, prompting until they do.
        /// </summary>
        /// <param name="allowZero">If 0 is an accepted input or not</param>
        /// <returns>Positive integer</returns>
        public static int PositiveInteger(bool allowZero)
        {
            return ValidateNumber<int>(VariableType.Integer, ValueType.Positive, allowZero);
        }

        /// <summary>
        /// Ensures user inputs a negative integer, prompting until they do.
        /// </summary>
        /// <param name="allowZero">If 0 is an accepted input or not</param>
        /// <returns>Negative integer</returns>
        public static int NegativeInteger(bool allowZero)
        {
            return ValidateNumber<int>(VariableType.Integer, ValueType.Negative, allowZero);
        }

        /// <summary>
        /// Ensures user inputs a double, prompting until they do.
        /// </summary>
        /// <returns>Double</returns>
        public static double Double()
        {
            return ValidateNumber<double>(VariableType.Double, ValueType.Any, true);
        }

        /// <summary>
        /// Ensures user inputs a positive double, prompting until they do.
        /// </summary>
        /// <param name="allowZero">If 0 is an accepted input or not</param>
        /// <returns>Positive double</returns>
        public static double PositiveDouble(bool allowZero)
        {
            return ValidateNumber<double>(VariableType.Double, ValueType.Positive, allowZero);
        }

        /// <summary>
        /// Ensures user inputs a negative double, prompting until they do.
        /// </summary>
        /// <param name="allowZero">If 0 is an accepted input or not</param>
        /// <returns>Negative double</returns>
        public static double NegativeDouble(bool allowZero)
        {
            return ValidateNumber<double>(VariableType.Double, ValueType.Negative, allowZero);
        }

        /// <summary>
        /// Ensures user inputs a decimal, prompting until they do.
        /// </summary>
        /// <returns>Decimal</returns>
        public static decimal Decimal()
        {
            return ValidateNumber<decimal>(VariableType.Decimal, ValueType.Any, true);
        }

        /// <summary>
        /// Ensures user inputs a positive decimal, prompting until they do.
        /// </summary>
        /// <param name="allowZero">If 0 is an accepted input or not</param>
        /// <returns>Positive decimal</returns>
        public static decimal PositiveDecimal(bool allowZero)
        {
            return ValidateNumber<decimal>(VariableType.Decimal, ValueType.Positive, allowZero);
        }

        /// <summary>
        /// Ensures user inputs a negative decimal, prompting until they do.
        /// </summary>
        /// <param name="allowZero">If 0 is an accepted input or not</param>
        /// <returns>Negative decimal</returns>
        public static decimal NegativeDecimal(bool allowZero)
        {
            return ValidateNumber<decimal>(VariableType.Decimal, ValueType.Negative, allowZero);
        }

        /// <summary>
        /// Ensures user inputs a string, prompting until they do, then trims it.
        /// </summary>
        /// <returns>Trimmed string</returns>
        public static string TrimmedString()
        {
            while (true)
            {
                string? userInput = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(userInput))
                {
                    string trimmedInput = userInput.Trim();
                    return trimmedInput;
                }

                DisplayNotValidInput(VariableType.String, ValueType.Any, true);

            }
        }

        /// <summary>
        /// Ensures user inputs a number of Int or Double type, prompting until they do.
        /// </summary>
        /// <typeparam name="T">int or double</typeparam>
        /// <param name="valueType">Any, Positive or Negative</param>
        /// <param name="allowZero">If 0 is an accepted input or not</param>
        /// <returns></returns>
        public static T Number<T>(VariableType variableType, ValueType valueType, bool allowZero) where T : INumber<T>
        {
            return ValidateNumber<T>(variableType, valueType, allowZero);
        }


        /// <summary>
        /// Performs the actual logic from its API methods.
        /// </summary>
        /// <typeparam name="T">Int or Double</typeparam>
        /// <param name="valueType">Any, Positive or negative</param>
        /// <param name="allowZero">If 0 is an accepted input or not</param>
        /// <returns></returns>
        private static T ValidateNumber<T>(VariableType variableType, ValueType valueType, bool allowZero) where T : INumber<T>
        {
            while (true)
            {
                string? userInput = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(userInput))
                {
                    string trimmedInput = userInput.Trim();

                    object? result = null;

                    // Check each possible variable type.
                    if (typeof(T) == typeof(int) && int.TryParse(trimmedInput, out int intResult))
                    {
                        result = intResult;
                    }
                    if (typeof(T) == typeof(double) && (double.TryParse(trimmedInput, out double doubleResult)))
                    {
                        result = doubleResult;
                    }
                    if (typeof(T) == typeof(float) && float.TryParse(trimmedInput, out float floatResult))
                    {
                        result = floatResult;
                    }
                    if (typeof(T) == typeof(decimal) && decimal.TryParse(trimmedInput, out decimal decimalResult))
                    {
                        result = decimalResult;
                    }
                    if (typeof(T) == typeof(long) && long.TryParse(trimmedInput, out long longResult))
                    {
                        result = longResult;
                    }
                    if (typeof(T) == typeof(short) && short.TryParse(trimmedInput, out short shortResult))
                    {
                        result = shortResult;
                    }
                    // Done checking variable types.


                    if (result != null && HasValidValue<T>((T)result!, valueType, allowZero))
                    {
                        return (T)result!;
                    }
                }

                DisplayNotValidInput(variableType, valueType, allowZero);
            }
        }


        /// <summary>
        /// Allows user to input a string value without displaying input on screen. Returns inputted string,
        /// allowing no whitespace or null input. 
        /// </summary>
        /// <param name="showStars">Show password-style stars (****) when inputting characters or not</param>
        /// <param name="maxLength">How many characters the input can be. Set 0 for no limit</param>
        /// <returns>User's string input</returns>
        public static string HiddenString(bool showStars, int maxLength)
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
                            Console.Write("\n");
                            isInputting = false;
                        }
                        break;

                    case ConsoleKey.Backspace:
                        if (characters > 0)
                        {
                            userInput = userInput.Substring(0, userInput.Length - 1);
                            if (showStars)
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
                            if (characters <= maxLength || maxLength == 0)
                            {
                                userInput += pressed.KeyChar;
                                characters++;
                                if (showStars)
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


        //
        //          - = Helper method internal methods = -
        //


        /// <summary>
        /// Checks whether the user inputted value corresponds to the desired parameters.
        /// </summary>
        /// <typeparam name="T">Int or Double</typeparam>
        /// <param name="userInput">The user submitted value</param>
        /// <param name="valueType">Any, Positive or Negative</param>
        /// <param name="allowZero">If 0 is an accepted input or not</param>
        /// <returns></returns>
        private static bool HasValidValue<T>(T userInput, ValueType valueType, bool allowZero) where T : INumber<T>
        {
            switch (valueType)
            {
                case ValueType.Positive:
                    if (userInput >= T.Zero && allowZero == true)
                    {
                        return true;
                    }
                    if (userInput > T.Zero && allowZero == false)
                    {
                        return true;
                    }
                    return false;

                case ValueType.Negative:
                    if (userInput <= T.Zero && allowZero == true)
                    {
                        return true;
                    }
                    if (userInput < T.Zero && allowZero == false)
                    {
                        return true;
                    }
                    return false;

                default:
                    return true;
            }
        }


        /// <summary>
        /// Error messages
        /// </summary>
        /// <param name="variableType">String or Number</param>
        /// <param name="valueType">Any, Positive or Negative</param>
        /// <param name="allowZero"></param>
        private static void DisplayNotValidInput(VariableType variableType, ValueType valueType, bool allowZero)
        {
            Thread.Sleep(200);
            if (variableType == VariableType.String)
            {
                Console.Write("Text input required");
            }
            else
            {
                Console.Write("Not a valid ");
                switch (valueType)
                {
                    case ValueType.Positive:
                        Console.Write("positive ");
                        PostAllowZero(allowZero);
                        break;
                    case ValueType.Negative:
                        Console.Write("negative ");
                        PostAllowZero(allowZero);
                        break;
                }
                Console.Write("number");
                if (variableType == VariableType.Integer)
                {
                    Console.Write(" with no decimals");
                }
            }
            Console.Write(". Try again: ");
        }



        private static void PostAllowZero(bool allowZero)
        {
            if (!allowZero)
            {
                Console.Write("(0 excluded) ");
            }
        }


        /// <summary>
        /// Expected inputted variable type. String, Integer or Double
        /// </summary>
        public enum VariableType
        {
            String,
            Integer,
            Double,
            Decimal,
            Number
        }

        /// <summary>
        /// Expected value: Any, Positive or Negative.
        /// </summary>
        public enum ValueType
        {
            Any,
            Positive,
            Negative
        }

    }
}

