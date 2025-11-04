using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace bank_app.Utility.UI.Components
{
    public class Dropdown<T> : UIComponent
    {
        public List<DropdownItem<T>> Items {  get; set; }
        private int _index = 0;
        public DropdownItem<T> SelectedDropdownItem { get; set; }
        public Dropdown(List<DropdownItem<T>> values)
        {
            IsInteractable = true;
            Items = values;
            Height = 1;
            if (Items.Count > 0)
            {
                SelectedDropdownItem = Items[0];
                Value = Items[0].Item;
            }
        }
        public override void Measure()
        {
            
        }
        public override (int, int) Pressed()
        {
            while (true)
            {
                for (int i = 0; i < Items.Count; i++)
                {
                    Console.SetCursorPosition(X, Y + i);
                    if (i == _index)
                    {
                        ColourManager.Write(Items[i].Name, ColourFG.Black, ColourBG.White);
                    }
                    else
                    {
                        Console.Write(Items[i].Name);
                    }
                }
                // Read key and if user presses up or down we add or subtract from i. If user presses Enter we run the components pressed method.
                ConsoleKey key = Console.ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.DownArrow:
                        if (_index < Items.Count - 1)
                        {
                            _index++;
                        }
                        else
                        {
                            CleanUp();
                            return (-1, 0);
                        }
                        break;
                    case ConsoleKey.UpArrow:
                        if (_index > 0)
                        {
                            _index--;
                        }
                        else
                        {
                            CleanUp();
                            return (1, 0);
                        }
                        break;
                    case ConsoleKey.RightArrow:
                        CleanUp();
                        return (0, 1);
                    case ConsoleKey.LeftArrow:
                        CleanUp();
                        return (0, -1);
                    case ConsoleKey.Enter:
                        SelectedDropdownItem = Items[_index];
                        Value = Items[_index].Item;
                        CleanUp();
                        return (0, 0);
                }
            }
        }
        private void CleanUp()
        {
            // This cleans either at root or at the closest Panel, might need to change depending on how complicated UI gets
            UIComponent? currentParentComponent = ParentComponent;
            while (currentParentComponent != null)
            {
                if (currentParentComponent is Panel panel)
                {
                    panel.Border = LayoutBorder.Ascii;
                    panel.Render();
                    return;
                }
                else if (currentParentComponent.ParentComponent == null)
                {
                    currentParentComponent.Render();
                }
                currentParentComponent = currentParentComponent.ParentComponent;
            }
        }
        public override void Render()
        {
            Console.SetCursorPosition(X,Y);
            Console.Write(SelectedDropdownItem.Name);
        }
    }
}
