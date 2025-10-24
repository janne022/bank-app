using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bank_app.Utility.Components
{
    public class Button : UIComponent
    {
        public Delegate Delegate { get; set; }
        public Button(Delegate del)
        {
            Delegate = del;
        }
        public override void Render() { }

        public void Pressed(params object[] args)
        {
            Delegate.DynamicInvoke(args);
        }
    }
}
