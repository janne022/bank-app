using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bank_app.Utility.Components
{
    public class Button : UIComponent
    {
        public string Text { get; private set; }
        public Delegate Delegate { get; set; }
        public Action<Button>? PressEvent { get; set; }
        public Button(Delegate delegation, string text)
        {
            Text = text;
            Delegate = delegation;
        }
        
        public void Pressed(params object[] args)
        {
            Delegate.DynamicInvoke(args);
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
            Console.SetCursorPosition(X, Y);
            Console.Write($"[ {Text} ]");
        }
    }
}
