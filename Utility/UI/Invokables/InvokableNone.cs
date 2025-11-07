namespace bank_app.Utility.UI.Invokables
{
    /// <summary>
    /// Does not use IInvokable because it doesn't have parameters. Will still overload fine.
    /// </summary>
    /// <param name="action">A reference to the method to run using a Action as generic delegate</param>
    public class Invokable(Action action)
    {
        private readonly Action _action = action;

        public void Invoke()
        {
            _action();
        }
    }
}
