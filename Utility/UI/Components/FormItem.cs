namespace bank_app.Utility.UI.Components
{
    public class FormItem : UIComponent
    {
        public UIComponent Component { get; set; }
        public bool IsFormValue { get; set; }

        public FormItem(UIComponent component, bool isFormValue)
        {
            Component = component;
            IsFormValue = isFormValue;
            Component.ParentComponent = this;
            IsInteractable = component.IsInteractable;
        }
        public override (int, int) Pressed()
        {
            Component.Pressed();
            return (0, 0);
        }

        public override void Measure()
        {
            Width = ParentComponent.Width;
            Height = ParentComponent.Height;
            Component.Measure();
            Height = Component.Height;
        }

        public override void Render()
        {
            Component.X = X;
            Component.Y = Y;
            Component.Render();
        }
    }
}
