using bank_app.Utility.UI.Invokables;

namespace bank_app.Utility.UI.Components
{
    /// <summary>
    /// Button UIComponent. Performs actions on page.
    /// </summary>
    public class Button : UIComponent
    {
        public string Text { get; private set; }
        private readonly IInvokable? _invokable;
        private readonly Invokable? _invokableNone;
        private Justify Alignment;
        private ColourFG _textColour;
        private ColourBG _buttonColour;


        /// <summary>
        /// Button UIComponent constructor.
        /// </summary>
        /// <param name="invokable">Needs to be an Invokable class object with the method you want to run</param>
        /// <param name="text">The text displayed on the button.</param>
        /// <param name="justify">Whether the button should be displayed aligned left, right or center.</param>
        /// <param name="textColour">Colour of text. Default to the terminal's default.</param>
        /// <param name="buttonColour">Colour of the button itself. Default to the terminal's default.</param>
        public Button(IInvokable invokable, string text, Justify justify = Justify.Center, ColourFG textColour = ColourFG.None, ColourBG buttonColour = ColourBG.None, int marginTop = 0, int marginLeft = 0, int marginRight = 0, int marginBottom = 0) : base(marginTop, marginLeft, marginRight, marginBottom)
        {
            Text = text;
            _invokable = invokable;
            Alignment = justify;
            _textColour = textColour;
            _buttonColour = buttonColour;
            IsInteractable = true;
        }

        /// <summary>
        /// Button UIComponent constructor.
        /// </summary>
        /// <param name="invokable">Needs to be an Invokable class object with the method you want to run</param>
        /// <param name="text">The text displayed on the button.</param>
        /// <param name="justify">Whether the button should be displayed aligned left, right or center.</param>
        /// <param name="textColour">Colour of text. Default to the terminal's default.</param>
        /// <param name="buttonColour">Colour of the button itself. Default to the terminal's default.</param>
        public Button(Invokable invokable, string text, Justify justify = Justify.Center, ColourFG textColour = ColourFG.None, ColourBG buttonColour = ColourBG.None, int marginTop = 0, int marginLeft = 0, int marginRight = 0, int marginBottom = 0) : base(marginTop, marginLeft, marginRight, marginBottom)
        {
            Text = text;
            _invokableNone = invokable;
            Alignment = justify;
            _textColour = textColour;
            _buttonColour = buttonColour;
            IsInteractable = true;
        }

        /// <summary>
        /// Press (on Enter key) button functionality with passed arguments.
        /// </summary>
        /// <param name="args">Object array sent as arguments</param>
        public override (int, int) Pressed(params object[] args)
        {
            _invokable?.Invoke(args);
            return (0, 0);
        }

        // Runs with a regular Invokable class that doesn't need any arguments
        public override (int, int) Pressed()
        {
            _invokableNone?.Invoke();
            return (0, 0);
        }

        public override void Measure()
        {
            Height = 1 + MarginTop + MarginBottom;
            Width = Text.Length + 4 + MarginRight + MarginLeft;  // [ ButtonText ]
                                                                 // 12          34
        }

        public override void Render()
        {
            int marginLeft = SetMarginLeft();

            Console.SetCursorPosition(X + marginLeft, Y);
            Console.Write("[");
            ColourManager.Write($" {Text} ", _textColour, _buttonColour);
            Console.Write("]");
        }

        private int SetMarginLeft()
        {
            switch (Alignment)
            {
                case Justify.Start:
                    return 0;

                case Justify.Center:
                    return (ParentComponent!.Width - Text.Length) / 2;

                case Justify.End:
                    return ParentComponent!.Width - Text.Length;
            }
            return 0;
        }
    }
}
