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
    public class Button : UIComponent
    {
        public string Text { get; private set; }
        public Delegate Delegate { get; set; }
        public Action<Button>? PressEvent { get; set; }
        private Justify ButtonJustify;

        public Button(Delegate delegation, string text, Justify justify = Justify.Center)
        {
            Text = text;
            Delegate = delegation;
            ButtonJustify = justify;
        }
        
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

        public override void Pressed()
        {
            // How to set what the button does:
            // myButton.PressEvent = myButton => myText.Render();
            // myButton.Pressed();
            PressEvent?.Invoke(this);
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
            switch (ButtonJustify)
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
