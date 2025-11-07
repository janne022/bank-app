namespace bank_app.Utility.UI.Invokables
{
    /// <summary>
    /// Implements IInvokable and takes in a method that has 1 parameter and does not return anything.
    /// </summary>
    /// <typeparam name="T1">Any type of parameter can be included that matches a method with 1 parameter</typeparam>
    /// <param name="action">A reference to the method to run using a Action as generic delegate</param>
    public class Invokable<T1>(Action<T1> action) : IInvokable
    {
        private readonly Action<T1> _action = action;

        public void Invoke(params object[] args)
        {
            _action((T1)args[0]);
        }
    }
}
