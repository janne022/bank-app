using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bank_app.Utility.UI.Components
{
    public struct DropdownItem<T>
    {
        public string Name { get; set; }
        public T Item { get; set; }

        public DropdownItem(string name, T item)
        {
            Name = name;
            Item = item;
        }
    }
}
