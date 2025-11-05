namespace bank_app.Utility.UI.Invokables
{
    /// <summary>
    /// Implements IInvokable and takes in a method that has 4 parameters and does not return anything.
    /// </summary>
    /// <typeparam name="T1">Any type of parameter</typeparam>
    /// <typeparam name="T2">Any type of parameter</typeparam>
    /// <typeparam name="T3">Any type of parameter</typeparam>
    /// <typeparam name="T4">Any type of parameter</typeparam>
    /// <param name="action">A reference to the method to run using a Action as generic delegate</param>
    public class Invokable<T1, T2, T3, T4>(Action<T1, T2, T3, T4> action) : IInvokable
    {
        private readonly Action<T1, T2, T3, T4> _action = action;

        public void Invoke(params object[] args)
        {
            _action((T1)args[0], (T2)args[1], (T3)args[2], (T4)args[3]);
        }
    }
}
