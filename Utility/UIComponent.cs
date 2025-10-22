using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bank_app.Utility
{
    public abstract class UIComponent
    {
        // Internal coordinates
        public int X {  get; set; }
        public int Y { get; set; }

        public string Name { get; set; }

        // Renders the component
        public abstract void Render();
        // Should run when user clicks component
        public virtual void OnEnter()
        {
        }
    }
}
