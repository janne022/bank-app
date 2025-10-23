using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bank_app.Utility.Components
{
    internal class Grid : UIComponent
    {
        List<UIComponent> Components { get; set; }
        Grid(List<UIComponent> components, Justify justify = Justify.Start, Align align = Align.Start, OrderBy orderBy = OrderBy.Row)
        {
            Components = components;
        }
        public override void Render()
        {
        }
    }
}
