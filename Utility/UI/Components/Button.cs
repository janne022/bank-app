using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace bank_app.Utility.UI.Components
{
    /// <summary>
    /// Button UIComponent. Performs actions on page.
    /// </summary>
    public class Button : UIComponent
    {
        public string Text { get; private set; }
        public Delegate Delegate { get; set; }
        public Action<Button>? MethodRunner { get; set; }
        private Justify Alignment;
        private ColourFG _textColour;
        private ColourBG _buttonColour;


        /// <summary>
        /// Button UIComponent constructor.
        /// </summary>
        /// <param name="delegation">The method to run on button press.</param>
        /// <param name="text">The text displayed on the button.</param>
        /// <param name="justify">Whether the button should be displayed aligned left, right or center.</param>
        /// <param name="textColour">Colour of text. Default to the terminal's default.</param>
        /// <param name="buttonColour">Colour of the button itself. Default to the terminal's default.</param>
        public Button(Delegate delegation, string text, Justify justify = Justify.Center, ColourFG textColour = ColourFG.None, ColourBG buttonColour = ColourBG.None)
        {
            Text = text;
            Delegate = delegation;
            Alignment = justify;
            _textColour = textColour;
            _buttonColour = buttonColour;
        }

        /// <summary>
        /// Press (on Enter key) button functionality with passed arguments.
        /// </summary>
        /// <param name="args">Object array sent as arguments</param>
        public void Pressed(params object[] args)
        {
            Delegate.DynamicInvoke(args);
        }

        public override void Measure()
        {
            Height = 1;
            Width = Text.Length + 4;  // [ ButtonText ]
                                      // 12          34
        }

        /// <summary>
        /// Press (on Enter key) button functionality with no passed arguments.
        /// Usage: myButton.MethodRunner = myButton => myObject.myMethod();
        /// </summary>
        public override void Pressed()
        {
            MethodRunner?.Invoke(this);
        }

        public override void Render()
        {
            int leftMargin = SetMargin();

            Console.SetCursorPosition(X + leftMargin, Y);
            Console.Write("[");
            ColourManager.Write($" {Text} ", ColourFG.BlueBright, ColourBG.Green);
            Console.Write("]");
        }

        private int SetMargin()
        {
            int leftMargin = 0;
            switch (Alignment)
            {
                case Justify.Start:
                    leftMargin = 0;
                    break;

                case Justify.Center:
                    leftMargin = (ParentElement!.Width - Text.Length) / 2;
                    break;

                case Justify.End:
                    leftMargin = ParentElement!.Width - Text.Length;
                    break;
            }
            return leftMargin;
        }
    }
}
