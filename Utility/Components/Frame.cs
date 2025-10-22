using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bank_app.Utility.Components
{
    public class Frame : UIComponent
    {
        List<UIComponent> Components {  get; set; }

        Frame(List<UIComponent> components, Justify justify = Justify.Start, Align align = Align.Start, OrderBy orderBy = OrderBy.Row)
        {
            Components = components;
        }
        public override void Render() { }
    }
}
