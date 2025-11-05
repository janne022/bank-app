namespace bank_app.Utility.UI.Invokables
{
    /// <summary>
    /// Implements IInvokable and takes in a method that has 0 parameters and does not return anything. This one needs an empty object array
    /// </summary>
    /// <param name="action">A reference to the method to run using a Action as generic delegate</param>
    public class Invokable(Action action) : IInvokable
    {
        private readonly Action _action = action;

        public void Invoke(params object[] args)
        {
            _action();
        }
    }
}
