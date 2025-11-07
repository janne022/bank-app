namespace bank_app.Utility.UI.Components
{
    public class Dropdown<T> : UIComponent
    {
        public List<DropdownItem<T>> DropdownItems { get; set; }
        public DropdownItem<T> SelectedDropdownItem { get; set; }
        ColourFG SelectionFG { get; set; }
        ColourBG SelectionBG { get; set; }
        ColourBG BackgroundBG { get; set; }
        public Action<T>? OnSelectionChanged { get; set; }
     
        public Dropdown(List<DropdownItem<T>> values, 
            ColourFG selectionFG = ColourFG.Black, ColourBG selectionBG = ColourBG.White, ColourBG backgroundBG = ColourBG.BlackBright, 
            int marginTop = 0, int marginLeft = 0, int marginRight = 0, int marginBottom = 0) 
            : base(marginTop, marginLeft, marginRight, marginBottom)
        {
            IsInteractable = true;
            IsMultiComponent = true;
            DropdownItems = values;
            SelectionFG = selectionFG;
            SelectionBG = selectionBG;
            BackgroundBG = backgroundBG;
            Height = 1;

            SelectedDropdownItem = DropdownItems[0];
            Value = DropdownItems[0].Item;

            Measure();
        }
        public override void Measure()
        {
            int largestDropdownName = 0;
            foreach (var item in DropdownItems)
            {
                if (item.Name.Length > largestDropdownName)
                {
                    largestDropdownName = item.Name.Length;
                }
            }
            Width = largestDropdownName + MarginLeft + MarginRight;
            Height = 1 + MarginTop + MarginBottom;
        }
        private string GetCenteredName(string name)
        {
            int totalPadding = Width - name.Length;
            int padLeftLength = (totalPadding / 2) + name.Length;
            return name.PadLeft(padLeftLength).PadRight(Width);
        }
        public override (int, int) Pressed()
        {
            int index = 0;
            while (true)
            {
                for (int i = -1; i < DropdownItems.Count; i++)
                {
                    Console.SetCursorPosition(X, Y + (i + 1));
                    string name = GetCenteredName(SelectedDropdownItem.Name);

                    if (i == -1)
                    {
                        if (i == index)
                        {
                            ColourManager.Write($"[ {name} ▾ ]", SelectionFG, SelectionBG);
                        }
                        else
                        {
                            ColourManager.Write($"[ {name} ▾ ]", ColourFG.Black, BackgroundBG);
                        }
                    }
                    else
                    {
                        name = GetCenteredName(DropdownItems[i].Name);
                        if (i == index)
                        {
                            ColourManager.Write($"{name}", SelectionFG, SelectionBG);
                        }
                        else
                        {
                            ColourManager.Write(name, ColourFG.Black, BackgroundBG);
                        }
                    }
                }
                // Read key and if user presses up or down we add or subtract from i. If user presses Enter we run the components pressed method.
                ConsoleKey key = Console.ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.DownArrow:
                        if (index < DropdownItems.Count - 1)
                        {
                            index++;
                            OnSelectionChanged?.Invoke(DropdownItems[index].Item);
                        }
                        break;
                    case ConsoleKey.UpArrow:
                        if (index > 0)
                        {
                            index--;
                            OnSelectionChanged?.Invoke(DropdownItems[index].Item);
                        }
                        break;
                    case ConsoleKey.Enter:
                        SelectedDropdownItem = DropdownItems[index];
                        Value = DropdownItems[index].Item;
                        OnSelectionChanged?.Invoke(DropdownItems[index].Item);
                        CleanUp();
                        return (0, 0);
                    case ConsoleKey.Escape:
                        CleanUp();
                        return (0, 0);
                }
            }
        }
        private void CleanUp()
        {
            // This cleans either at root
            UIComponent? currentParentComponent = ParentComponent;
            while (currentParentComponent != null)
            {
                if (currentParentComponent.ParentComponent == null)
                {
                    currentParentComponent.Render();
                }
                currentParentComponent = currentParentComponent.ParentComponent;
            }
        }
        public override void Render()
        {
            Console.SetCursorPosition(X, Y);
            string name = SelectedDropdownItem.Name;
            int totalPadding = Width - name.Length;
            int padLeftLength = (totalPadding / 2) + name.Length;
            string centeredName = name.PadLeft(padLeftLength).PadRight(Width);
            Console.Write($"[ {centeredName} ▸ ]");
        }
    }
}
