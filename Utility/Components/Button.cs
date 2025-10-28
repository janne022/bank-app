using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace bank_app.Utility.Components
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

        /// <summary>
        /// Button UIComponent constructor.
        /// </summary>
        /// <param name="delegation">The method to run on button press.</param>
        /// <param name="text">The text displayed on the button.</param>
        /// <param name="justify">Whether the button should be displayed aligned left, right or center.</param>
        public Button(Delegate delegation, string text, Justify justify = Justify.Center)
        {
            Text = text;
            Delegate = delegation;
            Alignment = justify;
        }
        
        /// <summary>
        /// Press (on Enter key) button functionality with passed arguments.
        /// </summary>
        /// <param name="args">Object array sent as arguments</param>
        public void Pressed(params object[] args)
        {
            Delegate.DynamicInvoke(args);
        }

        public void Measure()
        {
            Height = 1;
            Width = (Text.Length + 4);  // [ ButtonText ]
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

            Console.SetCursorPosition(X+leftMargin, Y);
            
            Console.Write($"[ {Text} ]");
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
                    leftMargin = ((ParentElement!.Width - Text.Length) / 2);
                    break;

                case Justify.End:
                    leftMargin = (ParentElement!.Width - Text.Length);
                    break;
            }
            return leftMargin;
        }
    }
}
