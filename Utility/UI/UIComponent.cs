using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bank_app.Utility.UI
{
    public abstract class UIComponent
    {
        // Declaring variables
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public bool IsInteractable = false;
        public object? Value { get; set; }
        public UIComponent? ParentElement { get; set; }


        /// <summary>
        /// Render should be used when displaying the component to the console
        /// </summary>
        public abstract void Render();

        /// <summary>
        /// Measure is an optional method used when setting the components width and height based on its parent width and height.
        /// It is used because C# required the creation of child components before parent objects
        /// </summary>
        public virtual void Measure()
        {
        }

        /// <summary>
        /// Pressed is a optional method used whenever a user is clicking inside another interactable component.
        /// </summary>
        /// 
        public virtual void Pressed()
        {
        }
    }
}
