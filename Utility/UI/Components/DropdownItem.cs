namespace bank_app.Utility.UI.Components
{
    public struct DropdownItem<T>
    {
        public string Name { get; set; }
        public T DropDownItem { get; set; }

        public DropdownItem(string name, T item)
        {
            Name = name;
            DropDownItem = item;
        }
    }
}
