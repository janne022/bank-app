using bank_app.Utility.Components;

namespace bank_app.Utility.UI.Components
{
    public class Form : UIComponent
    {
        private List<FormItem> _components;
        private object[] args;
        private int _index = 0;
        public ColourFG SelectionFG { get; set; }
        public ColourBG SelectionBG { get; set; }
        public Button SubmitButton { get; set; }
        public Text ErrorText { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="Form"/> that renders and handles
        /// keyboard navigation for a list of child <see cref="UIComponent"/>s.
        /// </summary>
        /// <param name="components">
        /// The child components to display inside the menu, in render/navigation order.
        /// The menu sets itself as each child's <see cref="UIComponent.ParentComponent"/>.
        /// </param>
        /// <param name="order">
        /// Intended layout ordering for the menu items (row or column). Rendering currently
        /// behaves as a column list; this parameter is reserved for future layout behavior.
        /// </param>
        /// <remarks>
        /// The menu is interactive by default. It highlights the currently selected item and
        /// supports Up/Down arrow navigation and Enter to activate the selected component.
        /// When a non-<see cref="Button"/> component is activated, its <see cref="UIComponent.Value"/>
        /// is captured internally. When a <see cref="Button"/> is activated, all captured values
        /// are passed to the button via <c>Pressed(object[] args)</c>.
        /// </remarks>
        public Form(List<UIComponent> components, Button submitButton, int width = 30, ColourFG selectionFG = ColourFG.Black, ColourBG selectionBG = ColourBG.White)
        {
            // Set variables
            IsInteractable = true;
            IsMultiComponent = true;
            _components = new List<FormItem>();
            Width = width;
            Height = components.Count;
            SelectionFG = selectionFG;
            SelectionBG = selectionBG;
            SubmitButton = submitButton;
            ErrorText = new Text("", textColour: ColourFG.RedBright);

            // Make this object a parent to all child objects
            foreach (var item in components)
            {
                var formItem = new FormItem(item, true);
                formItem.ParentComponent = this;
                _components.Add(formItem);
            }
            var formItemButton = new FormItem(SubmitButton, false);
            formItemButton.ParentComponent = this;
            _components.Add(formItemButton);
            var formItemText = new FormItem(ErrorText, false);
            formItemText.ParentComponent = this;
            _components.Add(formItemText);
            var formValueComponents = _components.Where(component => component.IsFormValue).ToList();
            int argsCount = formValueComponents.Count;
            args = new object[argsCount];
            for (int i = 0; i < args.Length; i++)
            {
                args[i] = formValueComponents[i].Component.Value;
            }
        }

        public override (int, int) Pressed()
        {
            // Holds a menu in a while loop
            while (true)
            {
                // Render every object after eachother. If selected object is the one we are rendering, we highlight it
                for (int j = 0; j < _components.Count; j++)
                {
                    if (j == _index)
                    {
                        // Black foreground on white background
                        ColourManager.Set(SelectionFG);
                        ColourManager.Set(SelectionBG);
                        _components[j].Render();
                        ColourManager.Set(ColourBG.Reset);
                        ColourManager.Set(ColourFG.Reset);
                    }
                    else
                    {
                        _components[j].Render();
                    }
                }
                // Read key and if user presses up or down we add or subtract from i. If user presses Enter we run the components pressed method.
                ConsoleKey key = Console.ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.DownArrow:
                        if (_index < _components.Count - 1)
                        {
                            int newIndex = _components.FindIndex(component => component.IsInteractable && _components.IndexOf(component) > _index);
                            if (newIndex != -1)
                            {
                                _index = newIndex;
                            }
                        }
                        else
                        {
                            return (1, 0);
                        }
                        break;
                    case ConsoleKey.UpArrow:
                        if (_index > 0)
                        {
                            int newIndex = _components.FindIndex(component => component.IsInteractable && _components.IndexOf(component) < _index);
                            if (newIndex != -1)
                            {
                                _index = newIndex;
                            }
                        }
                        else
                        {
                            return (-1, 0);
                        }
                        break;
                    case ConsoleKey.RightArrow:
                        return (0, 1);
                    case ConsoleKey.LeftArrow:
                        return (0, -1);
                    case ConsoleKey.Enter:
                        _components[_index].Pressed();
                        // If component is not a button we set the args index to be the value inside the component. If it is a button we run the pressed method for button with the current args.
                        if (_components[_index].IsFormValue == true)
                        {
                            args[_index] = _components[_index].Component.Value;
                        }
                        else if (_components[_index].Component == SubmitButton)
                        {
                            bool success = _components.TakeWhile(formItem => formItem.IsFormValue).All(component => component.Component.Value != null);
                            if (success)
                            {
                                UpdateErrorMessage(" ");
                                SubmitButton.Pressed(args);
                            }
                            else
                            {
                                UpdateErrorMessage("Error: Fill out entire form");
                            }
                            return (0, 0);
                        }
                        break;
                }
            }
        }
        public override void Measure()
        {
            if (Height == _components.Count)
            {
                for (int i = 0; i < _components.Count; i++)
                {
                    Height += _components[i].Height;
                }
            }
        }
        public override void Render()
        {
            int totalHeight = 0;
            for (int j = 0; j < _components.Count; j++)
            {
                _components[j].Measure();
                _components[j].X = X;
                _components[j].Y = Y;
                totalHeight += _components[j].Height;
                _components[j].Y += totalHeight - 2;
                _components[j].Render();
            }
        }

        public void UpdateErrorMessage(string errorMessage)
        {
            ErrorText.UpdateText(errorMessage);
        }
    }
}
